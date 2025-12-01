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
            try
            {
                // Conexión con la BBDD
                string DBpath = Server.MapPath("~/hotel.db");
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + DBpath + ";Version = 3;"))
                {
                    conn.Open();
                    string query = "SELECT profile FROM Users " +
                                   "WHERE username = '" + username.Text + "' " +
                                   "AND password = '" + password.Text + "'";
                    using (SQLiteCommand comm = new SQLiteCommand(query, conn))
                    {
                        SQLiteDataReader reader = comm.ExecuteReader();

                        if (reader.Read())
                        {
                            string profile = reader["profile"].ToString();

                            // Guardamos datos en sesión por si hacen falta en otras páginas
                            Session["username"] = username.Text;
                            Session["profile"] = profile;

                            // ⭐ Redirección según el tipo de perfil
                            if (profile == "receptionist")
                            {
                                Response.Redirect("ReceptionistPage.aspx");
                            }
                            else if (profile == "client")
                            {
                                Response.Redirect("ClientPage.aspx");
                            }
                            else
                            {
                                Label1.Text = "Perfil desconocido.";
                            }
                        }
                        else
                        {
                            Label1.Text = "Usuario o contraseña incorrectos.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Label1.Text = "Could not login";
            }
        }

        protected void password_TextChanged(object sender, EventArgs e)
        {

        }
    }
}