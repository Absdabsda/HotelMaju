using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hotel
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void username_TextChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Label1.Text = "";
            string nextPage = null;

            try
            {
                string DBpath = Server.MapPath("~/hotel.db");

                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + DBpath + ";Version=3;BusyTimeout=5000;"))
                {
                    conn.Open();

                    string query = "SELECT profile FROM Users " +
                                   "WHERE username = '" + username.Text + "' " +
                                   "AND password = '" + password.Text + "'";

                    using (SQLiteCommand comm = new SQLiteCommand(query, conn))
                    using (SQLiteDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string profile = reader["profile"].ToString();

                            Session["username"] = username.Text;
                            Session["profile"] = profile;

                            if (profile == "receptionist")
                                nextPage = "ReceptionistPage.aspx";
                            else if (profile == "client")
                                nextPage = "ClientPage.aspx";
                            else
                                Label1.Text = "Unknown profile";
                        }
                        else
                        {
                            Label1.Text = "Wrong username or password";
                        }
                    } 
                }     
            }
            catch
            {
                Label1.Text = "Could not login";
            }

            if (nextPage != null)
                Response.Redirect(nextPage, false);
        }


        protected void password_TextChanged(object sender, EventArgs e)
        {

        }
    }
}