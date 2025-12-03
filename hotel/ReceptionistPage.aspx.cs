using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Web.UI;
using System.Web.UI.WebControls;
using hotel.Models;

namespace hotel
{
    public partial class ReceptionistPage : System.Web.UI.Page
    {
        // =========================================================
        //              CONEXIÓN A LA BASE DE DATOS
        // =========================================================

        private string GetConnectionString()
        {
            string dbPath = Server.MapPath("~/hotel.db");
            return "Data Source=" + dbPath + ";Version=3;BusyTimeout=5000;";
        }

        // Pequeño helper para que las comillas no rompan el SQL
        private string Escape(string value)
        {
            return (value ?? "").Replace("'", "''");
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

        protected void btnAddClient_Click(object sender, EventArgs e)
        {
            lblClientMsg.Text = "";

            // 1) Validar ClientID numérico y > 0
            if (!int.TryParse(txtClientID.Text, out int clientID) || clientID <= 0)
            {
                lblClientMsg.Text = "Enter a valid positive numeric Client ID.";
                return;
            }

            // 2) Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtDOB.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtMobile.Text))
            {
                lblClientMsg.Text = "All fields (Name, DOB, Address, Phone) are required.";
                return;
            }

            // 3) Validar teléfono solo números y longitud razonable
            string mobileRaw = txtMobile.Text.Trim();

            if (!long.TryParse(mobileRaw, out _))
            {
                lblClientMsg.Text = "Phone must contain only numbers.";
                return;
            }

            if (mobileRaw.Length < 7 || mobileRaw.Length > 15)
            {
                lblClientMsg.Text = "Phone length must be between 7 and 15 digits.";
                return;
            }

            // 4) Validar que la fecha tenga formato correcto
            if (!DateTime.TryParse(txtDOB.Text, out _))
            {
                lblClientMsg.Text = "Invalid date format for DOB.";
                return;
            }

            // 5) Comprobar que NO exista ya un cliente con ese ClientID
            if (ClientExists(clientID))
            {
                lblClientMsg.Text = "There is already a client with that Client ID.";
                return;
            }

            // 6) Crear objeto Client con los datos del formulario
            Client client = new Client
            {
                ClientID = clientID,
                Name = txtName.Text.Trim(),
                DOB = txtDOB.Text,
                Address = txtAddress.Text.Trim(),
                Mobile = mobileRaw
            };

            // 7) Sanitizar strings para el SQL
            string name = Escape(client.Name);
            string dob = Escape(client.DOB);
            string address = Escape(client.Address);
            string mobile = Escape(client.Mobile);

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query =
                        "INSERT INTO Users (clientID, username, DOB, address, mobile) VALUES (" +
                        client.ClientID + ", '" +
                        name + "', '" +
                        dob + "', '" +
                        address + "', '" +
                        mobile + "')";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                lblClientMsg.Text = "Client created successfully.";
            }
            catch (Exception ex)
            {
                lblClientMsg.Text = "Error creating client: " + ex.Message;
            }
        }

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
        /// Carga los datos de un cliente en un objeto Client y rellena el formulario.
        /// También carga las reservas del cliente.
        /// </summary>
        private void LoadClientData(int clientID)
        {
            Client client = null;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query =
                        "SELECT userID, clientID, username, DOB, address, mobile " +
                        "FROM Users " +
                        "WHERE clientID = " + clientID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            client = new Client
                            {
                                UserID = Convert.ToInt32(reader["userID"]),
                                ClientID = Convert.ToInt32(reader["clientID"]),
                                Name = reader["username"].ToString(),
                                DOB = reader["DOB"].ToString(),
                                Address = reader["address"].ToString(),
                                Mobile = reader["mobile"].ToString()
                            };
                        }
                    }
                }

                if (client != null)
                {
                    // Rellenamos los TextBox con la clase
                    txtClientID.Text = client.ClientID.ToString();
                    txtName.Text = client.Name;
                    txtDOB.Text = client.DOB;
                    txtAddress.Text = client.Address;
                    txtMobile.Text = client.Mobile;

                    ViewState["selectedUserID"] = client.UserID;

                    lblSelectedClient.Text = $"Selected client: {client.Name} (ID {client.ClientID})";
                    lblClientMsg.Text = "Client found.";

                    // Cargamos sus reservas
                    LoadReservationsForClient(client.UserID);
                }
                else
                {
                    lblClientMsg.Text = "No client found with that ID.";
                }
            }
            catch (Exception)
            {
                lblClientMsg.Text = "Error searching client.";
            }
        }

        protected void btnUpdateClient_Click(object sender, EventArgs e)
        {
            lblClientMsg.Text = "";

            // 1) Validar ClientID numérico y > 0
            if (!int.TryParse(txtClientID.Text, out int clientID) || clientID <= 0)
            {
                lblClientMsg.Text = "Enter a valid positive Client ID to update.";
                return;
            }

            // 2) Comprobar que el cliente EXISTE antes de actualizar
            if (!ClientExists(clientID))
            {
                lblClientMsg.Text = "No client found with that ID. Cannot update.";
                return;
            }

            // 3) Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtDOB.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtMobile.Text))
            {
                lblClientMsg.Text = "All fields (Name, DOB, Address, Phone) are required to update the client.";
                return;
            }

            // 4) Validar teléfono
            string mobileRaw = txtMobile.Text.Trim();

            if (!long.TryParse(mobileRaw, out _))
            {
                lblClientMsg.Text = "Phone must contain only numbers.";
                return;
            }

            if (mobileRaw.Length < 7 || mobileRaw.Length > 15)
            {
                lblClientMsg.Text = "Phone length must be between 7 and 15 digits.";
                return;
            }

            // 5) Validar fecha
            if (!DateTime.TryParse(txtDOB.Text, out _))
            {
                lblClientMsg.Text = "Invalid date format for DOB.";
                return;
            }

            // 6) Crear objeto Client con los nuevos datos
            Client client = new Client
            {
                ClientID = clientID,
                Name = txtName.Text.Trim(),
                DOB = txtDOB.Text,
                Address = txtAddress.Text.Trim(),
                Mobile = mobileRaw
            };

            string name = Escape(client.Name);
            string dob = Escape(client.DOB);
            string address = Escape(client.Address);
            string mobile = Escape(client.Mobile);

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query =
                        "UPDATE Users SET " +
                        "username = '" + name + "', " +
                        "DOB = '" + dob + "', " +
                        "address = '" + address + "', " +
                        "mobile = '" + mobile + "' " +
                        "WHERE clientID = " + client.ClientID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                            lblClientMsg.Text = "Client updated successfully.";
                        else
                            lblClientMsg.Text = "No client was updated (unexpected).";
                    }
                }
            }
            catch (Exception ex)
            {
                lblClientMsg.Text = "Error updating client: " + ex.Message;
            }
        }


        protected void btnDeleteClient_Click(object sender, EventArgs e)
        {
            lblClientMsg.Text = "";

            // 1) Validar ClientID
            if (!int.TryParse(txtClientID.Text, out int clientID) || clientID <= 0)
            {
                lblClientMsg.Text = "Enter a valid positive Client ID to delete.";
                return;
            }

            // 2) Comprobar que el cliente EXISTE
            if (!ClientExists(clientID))
            {
                lblClientMsg.Text = "No client found with that ID. Nothing to delete.";
                return;
            }

            // 3) Obtener su userID interno
            int userID = GetUserIdByClientId(clientID);
            if (userID <= 0)
            {
                lblClientMsg.Text = "Internal error: could not find internal user ID.";
                return;
            }

            // 4) Comprobar si tiene reservas
            if (ClientHasReservations(userID))
            {
                lblClientMsg.Text = "This client still has reservations. Delete their reservations first.";
                return;
            }

            // 5) Borrar cliente
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = "DELETE FROM Users WHERE clientID = " + clientID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            lblClientMsg.Text = "Client deleted successfully.";
                            txtClientID.Text = "";
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
                            lblClientMsg.Text = "No client was deleted (unexpected).";
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

            string safeSearch = Escape(search);

            try
            {
                List<Client> clients = new List<Client>();

                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query =
                        "SELECT userID, clientID, username, DOB, address, mobile " +
                        "FROM Users " +
                        "WHERE username LIKE '%" + safeSearch + "%' " +
                        "   OR CAST(clientID AS TEXT) LIKE '%" + safeSearch + "%' " +
                        "ORDER BY username";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clients.Add(new Client
                            {
                                UserID = Convert.ToInt32(reader["userID"]),
                                ClientID = Convert.ToInt32(reader["clientID"]),
                                Name = reader["username"].ToString(),
                                DOB = reader["DOB"].ToString(),
                                Address = reader["address"].ToString(),
                                Mobile = reader["mobile"].ToString()
                            });
                        }
                    }
                }

                gvClients.DataSource = clients;
                gvClients.DataBind();

                if (clients.Count == 0)
                    lblSearchMsg.Text = "No clients found with that search.";
            }
            catch (Exception)
            {
                lblSearchMsg.Text = "Error searching clients.";
            }
        }

        protected void gvClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvClients.SelectedRow == null)
                return;

            GridViewRow row = gvClients.SelectedRow;

            // Asumimos que la primera columna es userID y la segunda es Name
            int userID = int.Parse(row.Cells[0].Text);
            string username = row.Cells[1].Text;

            // Creamos objeto Client mínimamente con lo que sabemos
            Client client = new Client
            {
                UserID = userID,
                Name = username
            };

            ViewState["selectedUserID"] = client.UserID;
            lblSelectedClient.Text = $"Selected client: {client.Name} (internal ID {client.UserID})";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query =
                        "SELECT clientID " +
                        "FROM Users " +
                        "WHERE userID = " + client.UserID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            int clientID = Convert.ToInt32(result);
                            LoadClientData(clientID); // carga panel cliente + reservas
                        }
                        else
                        {
                            LoadReservationsForClient(client.UserID);
                        }
                    }
                }
            }
            catch (Exception)
            {
                LoadReservationsForClient(client.UserID);
            }
        }

        // =========================================================
        //                      RESERVAS - CRUD
        // =========================================================

        private void LoadReservationsForClient(int userID)
        {
            gvReservations.DataSource = null;
            gvReservations.DataBind();
            ViewState["reservationID"] = null;

            try
            {
                List<Reservation> reservations = new List<Reservation>();

                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query =
                        "SELECT reservationID, userID, arrivalDate, departureDate, roomType " +
                        "FROM Reservations " +
                        "WHERE userID = " + userID +
                        " ORDER BY arrivalDate";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reservations.Add(new Reservation
                            {
                                ReservationID = Convert.ToInt32(reader["reservationID"]),
                                UserID = Convert.ToInt32(reader["userID"]),
                                ArrivalDate = reader["arrivalDate"].ToString(),
                                DepartureDate = reader["departureDate"].ToString(),
                                RoomType = reader["roomType"].ToString()
                            });
                        }
                    }
                }

                gvReservations.DataSource = reservations;
                gvReservations.DataBind();

                if (reservations.Count == 0)
                    lblReservationMsg.Text = "This client has no reservations yet.";
                else
                    lblReservationMsg.Text = $"Loaded {reservations.Count} reservations.";
            }
            catch (Exception)
            {
                lblReservationMsg.Text = "Error loading reservations.";
            }
        }

        protected void btnAddReservation_Click(object sender, EventArgs e)
        {
            lblReservationMsg.Text = "";

            if (ViewState["selectedUserID"] == null)
            {
                lblReservationMsg.Text = "Select a client first (search above and click Select).";
                return;
            }

            // Intentar parsear las fechas
            if (!DateTime.TryParse(txtArrival.Text, out DateTime arrivalDate) ||
                !DateTime.TryParse(txtDeparture.Text, out DateTime departureDate))
            {
                lblReservationMsg.Text = "Invalid date format for arrival or departure.";
                return;
            }

            // arrival < departure
            if (arrivalDate >= departureDate)
            {
                lblReservationMsg.Text = "Departure date must be after arrival date.";
                return;
            }

            // (Opcional) no permitir reservas en el pasado
            if (arrivalDate.Date < DateTime.Today)
            {
                lblReservationMsg.Text = "Arrival date cannot be in the past.";
                return;
            }


            int userID = (int)ViewState["selectedUserID"];

            if (string.IsNullOrWhiteSpace(txtArrival.Text) ||
                string.IsNullOrWhiteSpace(txtDeparture.Text))
            {
                lblReservationMsg.Text = "Enter arrival and departure dates.";
                return;
            }

            Reservation res = new Reservation
            {
                UserID = userID,
                ArrivalDate = txtArrival.Text,
                DepartureDate = txtDeparture.Text,
                RoomType = ddlRoomType.SelectedValue
            };

            string arrival = Escape(res.ArrivalDate);
            string departure = Escape(res.DepartureDate);
            string roomType = Escape(res.RoomType);

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query =
                        "INSERT INTO Reservations (userID, arrivalDate, departureDate, roomType) VALUES (" +
                        res.UserID + ", '" +
                        arrival + "', '" +
                        departure + "', '" +
                        roomType + "')";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
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

        protected void gvReservations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvReservations.SelectedRow == null)
                return;

            GridViewRow row = gvReservations.SelectedRow;

            // Creamos también objeto Reservation (por estilo)
            Reservation res = new Reservation
            {
                ReservationID = int.Parse(row.Cells[0].Text),
                ArrivalDate = row.Cells[1].Text,
                DepartureDate = row.Cells[2].Text,
                RoomType = row.Cells[3].Text,
                UserID = ViewState["selectedUserID"] != null
                         ? (int)ViewState["selectedUserID"]
                         : 0
            };

            ViewState["reservationID"] = res.ReservationID;

            txtArrival.Text = res.ArrivalDate;
            txtDeparture.Text = res.DepartureDate;

            if (ddlRoomType.Items.FindByValue(res.RoomType) != null)
                ddlRoomType.SelectedValue = res.RoomType;

            lblReservationMsg.Text = "Reservation selected. You can now update or delete it.";
        }

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

            // Intentar parsear las fechas
            if (!DateTime.TryParse(txtArrival.Text, out DateTime arrivalDate) ||
                !DateTime.TryParse(txtDeparture.Text, out DateTime departureDate))
            {
                lblReservationMsg.Text = "Invalid date format for arrival or departure.";
                return;
            }

            // arrival < departure
            if (arrivalDate >= departureDate)
            {
                lblReservationMsg.Text = "Departure date must be after arrival date.";
                return;
            }

            // (Opcional) no permitir reservas en el pasado
            if (arrivalDate.Date < DateTime.Today)
            {
                lblReservationMsg.Text = "Arrival date cannot be in the past.";
                return;
            }


            int userID = (int)ViewState["selectedUserID"];
            int reservationID = int.Parse(ViewState["reservationID"].ToString());

            Reservation res = new Reservation
            {
                ReservationID = reservationID,
                UserID = userID,
                ArrivalDate = txtArrival.Text,
                DepartureDate = txtDeparture.Text,
                RoomType = ddlRoomType.SelectedValue
            };

            string arrival = Escape(res.ArrivalDate);
            string departure = Escape(res.DepartureDate);
            string roomType = Escape(res.RoomType);

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query =
                        "UPDATE Reservations SET " +
                        "userID = " + res.UserID + ", " +
                        "arrivalDate = '" + arrival + "', " +
                        "departureDate = '" + departure + "', " +
                        "roomType = '" + roomType + "' " +
                        "WHERE reservationID = " + res.ReservationID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
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

            // También podemos crear un objeto Reservation por estilo
            Reservation res = new Reservation
            {
                ReservationID = reservationID,
                UserID = userID
            };

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query =
                        "DELETE FROM Reservations " +
                        "WHERE reservationID = " + res.ReservationID;

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

                LoadReservationsForClient(userID);
            }
            catch (Exception)
            {
                lblReservationMsg.Text = "Error deleting reservation.";
            }
        }

        // =========================================================
        //                    HELPERS
        // =========================================================
        private bool ClientExists(int clientID)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = "SELECT COUNT(*) FROM Users WHERE clientID = " + clientID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            long count = Convert.ToInt64(result);
                            return count > 0;
                        }
                    }
                }
            }
            catch
            {
                return true;
            }

            return false;
        }

        private int GetUserIdByClientId(int clientID)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = "SELECT userID FROM Users WHERE clientID = " + clientID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch
            {
                // Puedes loguear si quieres
            }

            return -1; // valor "no encontrado / error"
        }

        private bool ClientHasReservations(int userID)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query =
                        "SELECT COUNT(*) FROM Reservations " +
                        "WHERE userID = " + userID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            long count = Convert.ToInt64(result);
                            return count > 0;
                        }
                    }
                }
            }
            catch
            {
                // Si algo falla, mejor no borrar por si acaso
                return true;
            }

            return false;
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
