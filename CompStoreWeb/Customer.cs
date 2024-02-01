/*
 * Author: David Beltran
 * This file contains the Customer class. 
 */
namespace CompStoreWeb
{
    public class Customer
    {
        public string custName;
        public string? custEmail;
        public string? custPhone;
        public string? custAddress;

        public Customer(string name)
        {
            this.custName = name;
        }

        public Customer(string name, string email = "", string phone = "", string address = "")
        {
            this.custName = name;
            this.custEmail = email;
            this.custPhone = phone;
            this.custAddress = address;
        }
    }
}
