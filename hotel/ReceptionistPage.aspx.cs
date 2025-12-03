using System;
using System.Data;
using System.Data.SQLite;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hotel
{
    public partial class ReceptionistPage : System.Web.UI.Page
    {
        // =========================================================
        //              CONEXIÓN A LA BASE DE DATOS
        // =========================================================

        /// <summary>
        /// Construye el connection string para el SQLite del hotel.
        /// BusyTimeout ayuda a evitar errores de "database is locked".
        /// </summary>
        private string GetConnectionString()
        {
            string dbPath = Server.MapPath("~/hotel.db");
            return "Data Source=" + dbPath + ";Version=3;BusyTimeout=5000;";
        }

        // =========================================================
        //                    CICLO DE PÁGINA
        // =========================================================

        protected void Page_Load(object sender, EventArgs e)
        {
            // Seguridad básica: solo recepcionistas logueados
            if (Session["username"] == null)
                Response.Redirect("Login.aspx");

            if (Session["profile"] == null || Session["profile"].ToString() != "receptionist")
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                lblClientMsg.Text = "Tip: use Client ID to search and edit clients.";
                lblReservationMsg.Text = "Tip: search and select a client to see their reservations.";
                lblSelectedClient.Text = "No client selected yet.";
            }
        }

        // =========================================================
        //                      CLIENTES - CRUD
        // =========================================================

        /// <summary>
        /// Crea un nuevo cliente en la tabla Users.
        /// </summary>
        protected void btnAddClient_Click(object sender, EventArgs e)
        {
            lblClientMsg.Text = "";

            if (!int.TryParse(txtClientID.Text, out int clientID))
            {
                lblClientMsg.Text = "Enter a valid numeric Client ID.";
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = @"INSERT INTO Users (clientID, username, DOB, address, mobile)
                                     VALUES (@clientID, @username, @dob, @address, @mobile)";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@clientID", clientID);
                        cmd.Parameters.AddWithValue("@username", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@dob", txtDOB.Text);
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@mobile", txtMobile.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                lblClientMsg.Text = "Client created successfully.";
            }
            catch (Exception)
            {
                lblClientMsg.Text = "Error creating client.";
            }
        }

        /// <summary>
        /// Botón Search dentro del panel de client info (busca por ClientID).
        /// </summary>
        protected void btnFindClient_Click(object sender, EventArgs e)
        {
            lblClientMsg.Text = "";

            if (!int.TryParse(txtClientID.Text, out int clientID))
            {
                lblClientMsg.Text = "Enter a valid Client ID to search.";
                return;
            }

            LoadClientData(clientID);
        }

        /// <summary>
        /// Carga los datos de un cliente (UserID/ClientID) en los TextBox.
        /// </summary>
        private void LoadClientData(int clientID)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = @"SELECT userID, clientID, username, DOB, address, mobile
                                     FROM Users
                                     WHERE clientID = @clientID";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@clientID", clientID);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Rellenamos datos
                                txtClientID.Text = reader["clientID"].ToString();
                                txtName.Text = reader["username"].ToString();
                                txtDOB.Text = reader["DOB"].ToString();
                                txtAddress.Text = reader["address"].ToString();
                                txtMobile.Text = reader["mobile"].ToString();

                                // Guardamos el userID interno (PK) como cliente seleccionado
                                int userID = Convert.ToInt32(reader["userID"]);
                                ViewState["selectedUserID"] = userID;

                                lblSelectedClient.Text = $"Selected client: {reader["username"]} (ID {reader["clientID"]})";
                                lblClientMsg.Text = "Client found.";

                                // Cargamos sus reservas
                                LoadReservationsForClient(userID);
                            }
                            else
                            {
                                lblClientMsg.Text = "No client found with that ID.";
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                lblClientMsg.Text = "Error searching client.";
            }
        }

        /// <summary>
        /// Actualiza los datos de un cliente existente.
        /// </summary>
        protected void btnUpdateClient_Click(object sender, EventArgs e)
        {
            lblClientMsg.Text = "";

            if (!int.TryParse(txtClientID.Text, out int clientID))
            {
                lblClientMsg.Text = "Enter the Client ID of the client to update.";
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = @"UPDATE Users SET
                                        username = @username,
                                        DOB      = @dob,
                                        address  = @address,
                                        mobile   = @mobile
                                     WHERE clientID = @clientID";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@dob", txtDOB.Text);
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@mobile", txtMobile.Text.Trim());
                        cmd.Parameters.AddWithValue("@clientID", clientID);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                            lblClientMsg.Text = $"Client updated ({rows} row/s).";
                        else
                            lblClientMsg.Text = "No client found with that ID (nothing updated).";
                    }
                }
            }
            catch (Exception ex)
            {
                lblClientMsg.Text = "Error updating client: " + ex.Message;
            }
        }

        /// <summary>
        /// Elimina un cliente por ClientID.
        /// </summary>
        protected void btnDeleteClient_Click(object sender, EventArgs e)
        {
            lblClientMsg.Text = "";

            if (!int.TryParse(txtClientID.Text, out int clientID))
            {
                lblClientMsg.Text = "Enter the Client ID of the client you want to delete.";
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = @"DELETE FROM Users WHERE clientID = @clientID";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@clientID", clientID);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            lblClientMsg.Text = $"Client deleted ({rows} row/s).";
                            txtName.Text = "";
                            txtDOB.Text = "";
                            txtAddress.Text = "";
                            txtMobile.Text = "";
                            ViewState["selectedUserID"] = null;
                            lblSelectedClient.Text = "No client selected.";
                            gvReservations.DataSource = null;
                            gvReservations.DataBind();
                        }
                        else
                        {
                            lblClientMsg.Text = "No client found with that ID (nothing deleted).";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblClientMsg.Text = "Error deleting client: " + ex.Message;
            }
        }

        // =========================================================
        //           BUSCADOR Y SELECCIÓN DE CLIENTES (PANEL 1)
        // =========================================================

        /// <summary>
        /// Busca clientes por nombre o clientID (panel "Search client")
        /// y los muestra en gvClients.
        /// </summary>
        protected void btnSearchClient_Click(object sender, EventArgs e)
        {
            lblSearchMsg.Text = "";
            lblReservationMsg.Text = "";
            gvReservations.DataSource = null;
            gvReservations.DataBind();
            ViewState["selectedUserID"] = null;
            ViewState["reservationID"] = null;
            lblSelectedClient.Text = "No client selected yet.";

            string search = txtSearchClient.Text.Trim();

            if (string.IsNullOrEmpty(search))
            {
                lblSearchMsg.Text = "Enter a name or ID to search for clients.";
                gvClients.DataSource = null;
                gvClients.DataBind();
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = @"
                        SELECT userID, username, mobile
                        FROM Users
                        WHERE username LIKE @search
                           OR CAST(clientID AS TEXT) LIKE @search
                        ORDER BY username";

                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(query, conn))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@search", "%" + search + "%");

                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        gvClients.DataSource = dt;
                        gvClients.DataBind();

                        if (dt.Rows.Count == 0)
                            lblSearchMsg.Text = "No clients found with that search.";
                    }
                }
            }
            catch (Exception)
            {
                lblSearchMsg.Text = "Error searching clients.";
            }
        }

        /// <summary>
        /// Cuando seleccionas un cliente en gvClients:
        ///  - Guardamos su userID en ViewState.
        ///  - Cargamos sus datos en el panel de Client info.
        ///  - Cargamos sus reservas.
        /// </summary>
        protected void gvClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvClients.SelectedRow == null)
                return;

            GridViewRow row = gvClients.SelectedRow;

            // Columnas del GridView: 0=userID, 1=username, 2=mobile
            int userID = int.Parse(row.Cells[0].Text);
            string username = row.Cells[1].Text;

            ViewState["selectedUserID"] = userID;

            // Actualizamos etiqueta de cliente seleccionado (ID visible será el clientID que está en la tabla Users)
            lblSelectedClient.Text = $"Selected client: {username} (internal ID {userID})";

            // Cargamos datos del cliente a partir de clientID asociado
            // Primero obtenemos su clientID a partir del userID
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = @"SELECT clientID FROM Users WHERE userID = @userID";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userID", userID);

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            int clientID = Convert.ToInt32(result);
                            // Esto rellena el panel de cliente y también carga reservas
                            LoadClientData(clientID);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Si falla, al menos cargamos reservas con el userID
                LoadReservationsForClient(userID);
            }
        }

        // =========================================================
        //                      RESERVAS - CRUD
        // =========================================================

        /// <summary>
        /// Carga en el GridView todas las reservas de un cliente (userID).
        /// </summary>
        private void LoadReservationsForClient(int userID)
        {
            gvReservations.DataSource = null;
            gvReservations.DataBind();
            ViewState["reservationID"] = null;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = @"SELECT reservationID, arrivalDate, departureDate, roomType
                                     FROM Reservations
                                     WHERE userID = @userID
                                     ORDER BY arrivalDate";

                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(query, conn))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@userID", userID);

                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        gvReservations.DataSource = dt;
                        gvReservations.DataBind();

                        if (dt.Rows.Count == 0)
                            lblReservationMsg.Text = "This client has no reservations yet.";
                        else
                            lblReservationMsg.Text = $"Loaded {dt.Rows.Count} reservations.";
                    }
                }
            }
            catch (Exception)
            {
                lblReservationMsg.Text = "Error loading reservations.";
            }
        }

        /// <summary>
        /// Crea una nueva reserva para el cliente seleccionado (ViewState["selectedUserID"]).
        /// </summary>
        protected void btnAddReservation_Click(object sender, EventArgs e)
        {
            lblReservationMsg.Text = "";

            if (ViewState["selectedUserID"] == null)
            {
                lblReservationMsg.Text = "Select a client first (search above and click Select).";
                return;
            }

            int userID = (int)ViewState["selectedUserID"];

            if (string.IsNullOrWhiteSpace(txtArrival.Text) ||
                string.IsNullOrWhiteSpace(txtDeparture.Text))
            {
                lblReservationMsg.Text = "Enter arrival and departure dates.";
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = @"INSERT INTO Reservations (userID, arrivalDate, departureDate, roomType)
                                     VALUES (@userID, @arrival, @departure, @roomType)";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Parameters.AddWithValue("@arrival", txtArrival.Text);
                        cmd.Parameters.AddWithValue("@departure", txtDeparture.Text);
                        cmd.Parameters.AddWithValue("@roomType", ddlRoomType.SelectedValue);

                        cmd.ExecuteNonQuery();
                    }
                }

                lblReservationMsg.Text = "Reservation created successfully.";
                LoadReservationsForClient(userID);
            }
            catch (Exception)
            {
                lblReservationMsg.Text = "Error creating reservation.";
            }
        }

        /// <summary>
        /// Cuando seleccionas una reserva en gvReservations, se cargan sus datos
        /// en los TextBox para poder actualizar o borrar.
        /// </summary>
        protected void gvReservations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvReservations.SelectedRow == null)
                return;

            GridViewRow row = gvReservations.SelectedRow;

            string reservationID = row.Cells[0].Text;
            string arrival = row.Cells[1].Text;
            string departure = row.Cells[2].Text;
            string roomType = row.Cells[3].Text;

            ViewState["reservationID"] = reservationID;

            txtArrival.Text = arrival;
            txtDeparture.Text = departure;

            if (ddlRoomType.Items.FindByValue(roomType) != null)
                ddlRoomType.SelectedValue = roomType;

            lblReservationMsg.Text = "Reservation selected. You can now update or delete it.";
        }

        /// <summary>
        /// Actualiza la reserva seleccionada (reservationID en ViewState).
        /// </summary>
        protected void btnUpdateReservation_Click(object sender, EventArgs e)
        {
            lblReservationMsg.Text = "";

            if (ViewState["selectedUserID"] == null)
            {
                lblReservationMsg.Text = "Select a client first.";
                return;
            }

            if (ViewState["reservationID"] == null)
            {
                lblReservationMsg.Text = "Select a reservation in the table to update.";
                return;
            }

            int userID = (int)ViewState["selectedUserID"];
            int reservationID = int.Parse(ViewState["reservationID"].ToString());

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = @"UPDATE Reservations SET
                                        userID      = @userID,
                                        arrivalDate = @arrival,
                                        departureDate = @departure,
                                        roomType    = @roomType
                                     WHERE reservationID = @reservationID";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Parameters.AddWithValue("@arrival", txtArrival.Text);
                        cmd.Parameters.AddWithValue("@departure", txtDeparture.Text);
                        cmd.Parameters.AddWithValue("@roomType", ddlRoomType.SelectedValue);
                        cmd.Parameters.AddWithValue("@reservationID", reservationID);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                            lblReservationMsg.Text = "Reservation updated.";
                        else
                            lblReservationMsg.Text = "The reservation could not be updated.";
                    }
                }

                LoadReservationsForClient(userID);
            }
            catch (Exception)
            {
                lblReservationMsg.Text = "Error updating reservation.";
            }
        }

        /// <summary>
        /// Elimina la reserva seleccionada (reservationID en ViewState).
        /// </summary>
        protected void btnDeleteReservation_Click(object sender, EventArgs e)
        {
            lblReservationMsg.Text = "";

            if (ViewState["selectedUserID"] == null)
            {
                lblReservationMsg.Text = "Select a client first.";
                return;
            }

            if (ViewState["reservationID"] == null)
            {
                lblReservationMsg.Text = "Select a reservation first to delete.";
                return;
            }

            int userID = (int)ViewState["selectedUserID"];
            int reservationID = int.Parse(ViewState["reservationID"].ToString());

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = @"DELETE FROM Reservations WHERE reservationID = @reservationID";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@reservationID", reservationID);

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

                LoadReservationsForClient(userID);
            }
            catch (Exception)
            {
                lblReservationMsg.Text = "Error deleting reservation.";
            }
        }

        // =========================================================
        //                    LOGOUT
        // =========================================================

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}
