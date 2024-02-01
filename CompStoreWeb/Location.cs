/*
 * Author: David Beltran
 * This file contains the Location class. 
 */
using System.Runtime.CompilerServices;

namespace CompStoreWeb
{
    public class Location
    {
        public string? StoreID { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }

        public string GetInfo()
        {
            string info = "";
            if (StoreID != null && StoreID != "") {info += $"Store ID: {StoreID}";}
            if (Name != null && Name != "") { info += $" | Name: {Name}" ;}
            if (Address != null && Address != "") { info += $" | Address:{Address}"; }
            if (City != null && City != "") { info += $" | City:{City}"; }
            if (State != null && State != "") { info += $" | State:{State}"; }
            if (Zip != null && Zip != "") { info += $" | Zip:{Zip}"; }
            if (Latitude != null && Latitude != "") { info += $" | Latitude:{Latitude}"; }
            if (Longitude != null && Longitude != "") { info += $" | Longitude:{Longitude}"; }
            return info;
        }
    }
}
