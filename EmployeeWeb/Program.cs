/*
 * Author: David Beltran
 * This file contains methods that fill lists displayed on web pages. 
 */
using Microsoft.AspNetCore.Identity;
using System.Data.SQLite;
using System.Xml.Linq;
using System.Xml.XPath;

namespace EmployeeWeb
{
    internal class Program
    {
        public static List<Product> products = new List<Product>();
        public static List<Product> laptops = new List<Product>();
        public static List<Product> tablets = new List<Product>();
        public static List<Product> accessories = new List<Product>();
        public static List<Location> locations = new List<Location>();
        public static List<Customer> customers = new List<Customer>();
        public static string QuoteExcel = GetExcelSheet();
        public static List<string> users = new List<string>();
        static void Main(string[] args)
        {
            RetrieveDBData();
            GetLocations();
            GetCustomers();
            GetUsers();
            ShowWeb(args);
        }

        /// <summary>
        /// Retrieves products from database and utilizes LINQ to fill lists.
        /// </summary>
        public static void RetrieveDBData()
        {
            Database db = new Database();
            string query = "SELECT * FROM products";
            SQLiteCommand command = new SQLiteCommand(query, db.myConnection);
            db.OpenConnection();
            SQLiteDataReader result = command.ExecuteReader();

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

            laptops = products.Where(x => x.Type == "Laptop").ToList();
            tablets = products.Where(x => x.Type == "Tablet").ToList();
            accessories = products.Where(x => x.Type == "Accessory").ToList();
        }

        /// <summary>
        /// Fills lists with locations from XML file
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static void GetLocations()
        {
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
                catch (NullReferenceException ex)
                {
                    throw new ArgumentException("An element from XML file was null", ex);
                }
                locations.Add(newLocal);
            }
        }

        /// <summary>
        /// Fills list with customers from XML file
        /// </summary>
        public static void GetCustomers()
        {
            string currentPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            currentPath = Path.GetFullPath(Path.Combine(currentPath, @"Craft_A_Comp\CompStoreConsole\mailinglist\category.xml"));
            XElement customerXml = XElement.Load(currentPath);
            IEnumerable<XElement> shoppers = customerXml.Elements();

            foreach (XElement customer in shoppers)
            {
                Customer cust = new Customer();
                if (customer.XPathSelectElement("Name") != null) { cust.Name = customer.Element("Name")!.Value; }
                if (customer.XPathSelectElement("Address") != null) { cust.Address = customer.Element("Address")!.Value; }
                if (customer.XPathSelectElement("Email") != null) { cust.Email = customer.Element("Email")!.Value; }
                if (customer.XPathSelectElement("Phone") != null) { cust.Phone = customer.Element("Phone")!.Value; }
                if (customer.XPathSelectElement("FavCategory") != null) { cust.FavCategory = customer.Element("FavCategory")!.Value; }
                customers.Add(cust);
            }

        }

        /// <summary>
        /// Fills list with username only from XML file
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static void GetUsers()
        {
            string currentPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            currentPath = Path.GetFullPath(Path.Combine(currentPath, @"Craft_A_Comp\CompStoreConsole\loginCredentials\users.xml"));
            try
            {
                XElement userXml = XElement.Load(currentPath);
                IEnumerable<XElement> employees = userXml.Elements();
                foreach (XElement employee in employees)
                {
                    if (employee.XPathSelectElement("Username") != null) { users.Add(employee.Element("Username")!.Value); }
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new ArgumentException("The user.xml file was not found. Check console app to create file", ex);
            }
        }

        /// <summary>
        /// Excel sheet is returned to be downloaded by web page.
        /// </summary>
        /// <returns>Excel file that contains customer orders</returns>
        public static string GetExcelSheet()
        {
            string currentPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            return Path.GetFullPath(Path.Combine(currentPath, @"Craft_A_Comp\CompStoreConsole\quotes\customerQuotes.xlsx"));
        } 

        public static void ShowWeb(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            var app = builder.Build();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
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
// helped with downloading excel file
//https://www.syncfusion.com/kb/7866/download-excel-from-ajax-call-in-asp-net-mvc