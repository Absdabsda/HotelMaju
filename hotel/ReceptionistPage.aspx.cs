using System;
using System.Data;
using System.Data.SQLite;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hotel
{
    public partial class ReceptionistPage : System.Web.UI.Page
    {
        private string GetConnectionString()
        {
            string dbPath = Server.MapPath("~/hotel.db");
            return "Data Source=" + dbPath + ";Version=3;";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Si no hay sesión → fuera
            if (Session["username"] == null)
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
                LoadUsersDropDown();
        }


        // =======================
        //   CLIENTES
        // =======================

        private void LoadUsersDropDown()
        {
            ddlUsers.Items.Clear();

            using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();
                string query = "SELECT userID, username FROM Users ORDER BY username";

                using (SQLiteDataAdapter da = new SQLiteDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ddlUsers.DataSource = dt;
                    ddlUsers.DataTextField = "username";   // lo que ve el recepcionista
                    ddlUsers.DataValueField = "userID";    // lo que se usa para reservas
                    ddlUsers.DataBind();
                }
            }

            ddlUsers.Items.Insert(0, new ListItem("-- Selecct client --", ""));
        }

        protected void btnAddClient_Click(object sender, EventArgs e)
        {
            lblClientMsg.Text = "";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    // Insertamos usando clientID como identificativo propio
                    string query = "INSERT INTO Users (clientID, username, DOB, address, mobile) " +
                                   "VALUES (" + txtClientID.Text + ", '" + txtName.Text + "', '" +
                                              txtDOB.Text + "', '" + txtAddress.Text + "', '" +
                                              txtMobile.Text + "')";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                lblClientMsg.Text = "Client created succesfully.";
                LoadUsersDropDown();
            }
            catch (Exception ex)
            {
                lblClientMsg.Text = "Error creating a client.";
            }
        }

        protected void btnFindClient_Click(object sender, EventArgs e)
        {
            lblClientMsg.Text = "";

            if (string.IsNullOrWhiteSpace(txtClientID.Text))
            {
                lblClientMsg.Text = "Enter an identification number (clientID).";
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();
                    string query = "SELECT username, DOB, address, mobile " +
                                   "FROM Users WHERE clientID = " + txtClientID.Text;

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtName.Text = reader["username"].ToString();
                            txtDOB.Text = reader["DOB"].ToString();
                            txtAddress.Text = reader["address"].ToString();
                            txtMobile.Text = reader["mobile"].ToString();

                            lblClientMsg.Text = "Client found.";
                        }
                        else
                        {
                            lblClientMsg.Text = "No customer was found with that identifier.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblClientMsg.Text = "Error when searching for client.";
            }
        }

        protected void btnUpdateClient_Click(object sender, EventArgs e)
        {
            lblClientMsg.Text = "";

            if (string.IsNullOrWhiteSpace(txtClientID.Text))
            {
                lblClientMsg.Text = "Introduce el número identificativo del cliente que quieres actualizar.";
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    // IMPORTANTE: clientID tiene que ser un número entero existente
                    string query = "UPDATE Users SET " +
                                   "username = '" + txtName.Text + "', " +
                                   "DOB = '" + txtDOB.Text + "', " +
                                   "address = '" + txtAddress.Text + "', " +
                                   "mobile = '" + txtMobile.Text + "' " +
                                   "WHERE clientID = " + txtClientID.Text;

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        int rows = cmd.ExecuteNonQuery();   // cuántas filas ha tocado

                        if (rows > 0)
                        {
                            lblClientMsg.Text = "Cliente actualizado correctamente. (" + rows + " fila/s)";
                            LoadUsersDropDown();
                        }
                        else
                        {
                            lblClientMsg.Text = "No se ha encontrado ningún cliente con ese identificativo (no se actualizó nada).";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TEMPORALMENTE, para ver el error real:
                lblClientMsg.Text = "Error al actualizar cliente: " + ex.Message;
            }
        }


        protected void btnDeleteClient_Click(object sender, EventArgs e)
        {
            lblClientMsg.Text = "";

            if (string.IsNullOrWhiteSpace(txtClientID.Text))
            {
                lblClientMsg.Text = "Introduce el número identificativo del cliente que quieres eliminar.";
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = "DELETE FROM Users WHERE clientID = " + txtClientID.Text;

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            lblClientMsg.Text = "Cliente eliminado correctamente. (" + rows + " fila/s)";
                            txtName.Text = "";
                            txtDOB.Text = "";
                            txtAddress.Text = "";
                            txtMobile.Text = "";
                            LoadUsersDropDown();
                        }
                        else
                        {
                            lblClientMsg.Text = "No se ha encontrado ningún cliente con ese identificativo (no se eliminó nada).";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblClientMsg.Text = "Error al eliminar cliente: " + ex.Message;
            }
        }


        // =======================
        //   RESERVAS
        // =======================

        protected void btnAddReservation_Click(object sender, EventArgs e)
        {
            lblReservationMsg.Text = "";

            if (string.IsNullOrEmpty(ddlUsers.SelectedValue))
            {
                lblReservationMsg.Text = "Select a client for the booking.";
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    // En tu tabla: reservationID (PK autoincrement?), userID, arrivalDate, departureDate, roomType
                    string query = "INSERT INTO Reservations (userID, arrivalDate, departureDate, roomType) " +
                                   "VALUES (" + ddlUsers.SelectedValue + ", '" + txtArrival.Text + "', '" +
                                                txtDeparture.Text + "', '" + ddlRoomType.SelectedValue + "')";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                lblReservationMsg.Text = "Reservation created successfully.";
            }
            catch (Exception ex)
            {
                lblReservationMsg.Text = "Error creating a reservation.";
            }
        }

        protected void btnFindReservation_Click(object sender, EventArgs e)
        {
            lblReservationMsg.Text = "";

            if (string.IsNullOrEmpty(ddlUsers.SelectedValue))
            {
                lblReservationMsg.Text = "Select a client.";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtArrival.Text) || string.IsNullOrWhiteSpace(txtDeparture.Text))
            {
                lblReservationMsg.Text = "Enter arrival and departure dates to search for the reservation.";
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    // Buscamos una reserva de ese cliente con esas fechas
                    string query = "SELECT reservationID, roomType " +
                                   "FROM Reservations " +
                                   "WHERE userID = " + ddlUsers.SelectedValue +
                                   " AND arrivalDate = '" + txtArrival.Text + "'" +
                                   " AND departureDate = '" + txtDeparture.Text + "'";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Guardamos el ID de la reserva en ViewState para poder actualizar/borrar luego
                            ViewState["reservationID"] = reader["reservationID"].ToString();

                            string roomType = reader["roomType"].ToString();
                            if (ddlRoomType.Items.FindByValue(roomType) != null)
                            {
                                ddlRoomType.SelectedValue = roomType;
                            }

                            lblReservationMsg.Text = "Reservation found";
                        }
                        else
                        {
                            lblReservationMsg.Text = "No reservations were found with this information.";
                            ViewState["reservationID"] = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblReservationMsg.Text = "Error when searching for reservation.";
            }
        }

        protected void btnUpdateReservation_Click(object sender, EventArgs e)
        {
            lblReservationMsg.Text = "";

            if (ViewState["reservationID"] == null)
            {
                lblReservationMsg.Text = "First, find the reservation you want to update.";
                return;
            }

            try
            {
                string reservationID = ViewState["reservationID"].ToString();

                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = "UPDATE Reservations SET " +
                                   "userID = " + ddlUsers.SelectedValue + ", " +
                                   "arrivalDate = '" + txtArrival.Text + "', " +
                                   "departureDate = '" + txtDeparture.Text + "', " +
                                   "roomType = '" + ddlRoomType.SelectedValue + "' " +
                                   "WHERE reservationID = " + reservationID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            lblReservationMsg.Text = "Reservation updated.";
                        }
                        else
                        {
                            lblReservationMsg.Text = "The reservation could not be updated.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblReservationMsg.Text = "Error updating reservation.";
            }
        }

        protected void btnDeleteReservation_Click(object sender, EventArgs e)
        {
            lblReservationMsg.Text = "";

            if (ViewState["reservationID"] == null)
            {
                lblReservationMsg.Text = "First, find the reservation you want to delete..";
                return;
            }

            try
            {
                string reservationID = ViewState["reservationID"].ToString();

                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = "DELETE FROM Reservations WHERE reservationID = " + reservationID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            lblReservationMsg.Text = "Reservation removed.";
                            ViewState["reservationID"] = null;
                        }
                        else
                        {
                            lblReservationMsg.Text = "The reservation could not be deleted.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblReservationMsg.Text = "Error deleting reservation.";
            }
        }


        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();      // borra todas las variables de sesión
            Session.Abandon();    // invalida la sesión actual

            Response.Redirect("Login.aspx");
        }

    }
}
