using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hotel.Models
{
    public class Client
    {
        public int ClientID { get; set; }   // tu número identificativo
        public string Name { get; set; }    // Users.username
        public string DOB { get; set; }     // lo manejas como texto (yyyy-MM-dd)
        public string Address { get; set; }
        public string Mobile { get; set; }

        // Opcional: si quieres mapear también el userID de la tabla:
        public int? UserID { get; set; }
    }
}