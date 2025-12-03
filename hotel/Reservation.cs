using System;
using System.Collections.Generic;
using System.Linq;
using System;

namespace hotel.Models
{
    internal class Reservation
    {
        // ===== Fields =====
        private int reservationID;
        private int userID;
        private string arrivalDate;
        private string departureDate;
        private string roomType;

        // ===== Properties =====
        public int ReservationID
        {
            get { return reservationID; }
            set { reservationID = value; }
        }

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public string ArrivalDate
        {
            get { return arrivalDate; }
            set { arrivalDate = value; }
        }

        public string DepartureDate
        {
            get { return departureDate; }
            set { departureDate = value; }
        }

        public string RoomType
        {
            get { return roomType; }
            set { roomType = value; }
        }

        // ===== Constructor =====
        public Reservation() { }
        public Reservation(int reservationID, int userID, string arrivalDate, string departureDate, string roomType)
        {
            ReservationID = reservationID;
            UserID = userID;
            ArrivalDate = arrivalDate;
            DepartureDate = departureDate;
            RoomType = roomType;
        }

        // ===== Optional Utility Methods =====
        public string GetReservationInfo()
        {
            return
                $"RESERVATION INFO:\n" +
                $"Reservation ID: {ReservationID}\n" +
                $"User ID: {UserID}\n" +
                $"Room Type: {RoomType}\n" +
                $"Arrival: {ArrivalDate}\n" +
                $"Departure: {DepartureDate}\n";
        }
    }
}