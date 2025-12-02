using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SQLite;

namespace hotel
{
    public partial class ClientPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadClientData();
        }

        private void LoadClientData()
        {
            string username = Session["username"].ToString();

            string DBpath = Server.MapPath("~/hotel.db");
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + DBpath + ";Version=3;"))
            {
                conn.Open();

                // 1. Datos del cliente
                string q1 = "SELECT userID, DOB, address, mobile FROM Users WHERE username = '" + username + "'";
                SQLiteCommand c1 = new SQLiteCommand(q1, conn);
                SQLiteDataReader r1 = c1.ExecuteReader();

                string userId = "";

                if (r1.Read())
                {
                    lblName.Text = "Name: " + username; // no tienes campo Name
                    lblID.Text = "ID: " + r1["userID"].ToString(); // este sí existe
                    lblDOB.Text = "DOB: " + r1["DOB"].ToString();
                    lblAddress.Text = "Address: " + r1["address"].ToString();
                    lblMobile.Text = "Mobile: " + r1["mobile"].ToString();

                    userId = r1["userID"].ToString();
                }
                r1.Close();

                // 2. Reservas del cliente
                string q2 = "SELECT arrivalDate, departureDate, roomType FROM Reservations WHERE userID = " + userId;
                SQLiteCommand c2 = new SQLiteCommand(q2, conn);

                DataTable dt = new DataTable();
                dt.Load(c2.ExecuteReader());

                gvReservations.DataSource = dt;
                gvReservations.DataBind();
            }
        }

    }
}
