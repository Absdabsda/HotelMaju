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
                    string query = "SELECT profile FROM credentials WHERE name =" + name.Text + "AND password =" + password.Text;
                    using (SQLiteCommand comm = new SQLiteCommand(query, conn))
                    {
                        SQLiteDataReader reader = comm.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        foreach (DataRow dr in dt.Rows)
                        {
                            string role = dr["name"].ToString().ToLower();

                            
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