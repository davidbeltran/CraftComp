/*
 * Author: David Beltran
 * This file contains the Database class. 
 */
using System.Data.SQLite;
using System.IO;

namespace CompStoreWeb
{
    public class Database
    {
        public SQLiteConnection myConnection;
        /// <summary>
        /// Constructor connects object to a database
        /// </summary>
        public Database()
        {
            myConnection = new SQLiteConnection("Data Source=store_product.db");
            if (!File.Exists("./store_product.db"))
            {
                SQLiteConnection.CreateFile("store_product.db");
                System.Console.WriteLine("Database File created.");
            }
            else
                System.Console.WriteLine("Database file already exists.");
        }
        /// <summary>
        /// Method to open connection with database object
        /// </summary>
        public void OpenConnection()
        {
            if (myConnection.State != System.Data.ConnectionState.Open)
            {
                myConnection.Open();
            }
        }
        /// <summary>
        /// Method to close connection with database object
        /// </summary>
        public void CloseConnection()
        {
            if (myConnection.State != System.Data.ConnectionState.Closed)
            {
                myConnection.Close();
            }
        }
    }
}
