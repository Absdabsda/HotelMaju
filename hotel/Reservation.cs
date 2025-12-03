using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hotel.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }   // PK de Reservations
        public int UserID { get; set; }
        public string ArrivalDate { get; set; }      // texto tipo "2025-12-03"
        public string DepartureDate { get; set; }
        public string RoomType { get; set; }         // Individual / Doble / Suite

        // Si en tu tabla tienes RoomID más adelante, lo usas:
        public int? RoomID { get; set; }
    }
}