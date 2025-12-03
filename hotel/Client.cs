using System;
using System.Collections.Generic;
using System.Linq;

namespace hotel.Models
{
    internal class Client
    {

        private int clientID;
        private string name;
        private string dob;
        private string address;
        private string mobile;
        private int userID;


        public int ClientID
        {
            get { return clientID; }
            set { clientID = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string DOB
        {
            get { return dob; }
            set { dob = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        
        public Client() { }
        public Client(int clientID, string name, string dob, string address, string mobile, int userID)
        {
            ClientID = clientID;
            Name = name;
            DOB = dob;
            Address = address;
            Mobile = mobile;
            UserID = userID;
        }

       
        public string GetClientInfo()
        {
            return
                $"CLIENT INFO:\n" +
                $"ID: {ClientID}\n" +
                $"Name: {Name}\n" +
                $"DOB: {DOB}\n" +
                $"Address: {Address}\n" +
                $"Mobile: {Mobile}\n" +
                $"UserID: {UserID}";
        }
    }
}