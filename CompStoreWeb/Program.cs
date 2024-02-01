/*
 * Author: David Beltran
 * This file contains the Main() method that runs a brief introduction of 
 * the computer store application and displays store information to web page
 * on Index.cshtml file. 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;
using CompStoreWeb;
using System.Data.SQLite;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CompStoreWeb
{
    internal class Program
    {
        public static string[] merchandise = {"Desktops", "Laptops", "Cell Phones", "Tablets",
                "Motherboards", "Processors(CPU)", "Hard Drives and SSDs", "RAM Memory Kits"};

        public static string[,] desktops = { {"DELL", "Black", "$1500" }, 
            {"APPLE", "Silver", "$1700" }, {"MICROSOFT", "Blue", "$2000"} };

        public static Queue<Customer> waitingList = new Queue<Customer>();

        public static List<Product> products= new List<Product>();

        public static List<Product> laptops = new List<Product>();

        public static List<Product> tablets = new List<Product>();

        public static List<Product> accessories = new List<Product>();

        public static List<Location> locations = new List<Location>();

        static void Main(string[] args)
        {
            waitingList.Enqueue(new Customer("Phill Brooks"));
            waitingList.Enqueue(new Customer("Maxwell Jacob Friedman", "mjf@aew.com"));
            waitingList.Enqueue(new Customer("Donovan Danhausen", 
                address: "123 Very Nice Very Evil Rd."));

            Database databaseObject= new Database();
            string query = "SELECT * FROM products";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            databaseObject.OpenConnection();

            SQLiteDataReader result = myCommand.ExecuteReader();
            
            // Store each row from database as Product object
            if (result.HasRows)
            {
                while (result.Read())
                {
                    products.Add(new Product(result["name"].ToString()!, 
                        result["description"].ToString()!, result["brand"].ToString()!, 
                        Convert.ToDouble(result["price"].ToString()),
                        result["type"].ToString()!));
                }
            }

            // LINQ queries to store each product type into appropriate lists
            laptops = products.Where(x => x.Type == "Laptop").ToList();
            tablets = products.Where(x => x.Type == "Tablet").ToList();
            accessories = products.Where(x => x.Type == "Accessory").ToList();

            //Uses LINQ XElement to retrieve XML information and store in List<Location>
            XElement locationStuff = XElement.Load(@"store_locations.txt");
            IEnumerable<XElement> stores = locationStuff.Elements();
            
            foreach (XElement store in stores)
            {
                Location newLocal = new Location();
                try
                {
                    if (store.XPathSelectElement("storeid") != null) { newLocal.StoreID = store.Element("storeid")!.Value; }
                    if (store.XPathSelectElement("location") != null) { newLocal.Name = store.Element("location")!.Value; }
                    if (store.XPathSelectElement("address") != null) { newLocal.Address = store.Element("address")!.Value; }
                    if (store.XPathSelectElement("city") != null) { newLocal.City = store.Element("city")!.Value; }
                    if (store.XPathSelectElement("state") != null) { newLocal.State = store.Element("state")!.Value; }
                    if (store.XPathSelectElement("zip") != null) { newLocal.Zip = store.Element("zip")!.Value; }
                    if (store.XPathSelectElement("lat") != null) { newLocal.Latitude = store.Element("lat")!.Value; }
                    if (store.XPathSelectElement("lon") != null) { newLocal.Longitude = store.Element("lon")!.Value; }
                }
                catch(NullReferenceException ex)
                {
                    throw new ArgumentException("An element from XML file was null", ex);
                }
                locations.Add(newLocal);
            }



            ShowWeb(args);
        }
        
        public static void ShowWeb(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}


/*
 * https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/serialize-object-xml
 */