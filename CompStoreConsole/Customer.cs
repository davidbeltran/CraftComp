/*
 * Author: David Beltran
 * This file contains the Customer class. 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompStoreConsole
{
    /// <summary>
    /// Set as serializable to allow XML object storage.
    /// </summary>
    [Serializable]
    public class Customer
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FavCategory { get; set; }

        public string Display()
        {
            return $"{Name} | {Address} | {Email} | {Phone} | {FavCategory}";
        }
    }
}
