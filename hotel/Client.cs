using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hotel.Models
{
    public class Client
    {
        public int ClientID { get; set; }  
        public string Name { get; set; }    
        public string DOB { get; set; }     
        public string Address { get; set; }
        public string Mobile { get; set; }

        public int? UserID { get; set; }
    }
}