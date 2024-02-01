/*
 * Author: David Beltran
 * This file contains the Main() method that runs a brief introduction of 
 * the computer store application and allows the user to choose between 
 * viewing current orders or adding a customer to an XML file. 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using Excel = Microsoft.Office.Interop.Excel;

namespace CompStoreConsole
{
    internal class Program
    {
        static List<Computer> orders = new List<Computer>();
        static void Main(string[] args)
        {
            Introduction();

            FillOrders();

            GivenOptions();

            Console.ReadLine();
        }
        /// <summary>
        /// Introduces store to employee
        /// </summary>
        public static void Introduction()
        {
            Console.WriteLine("CRAFT-A-COMP");
            Console.WriteLine("Owner: DAVID BELTRAN");
            Console.WriteLine("Your stop for pre-built customizable computers and accessories!");
            Console.Write("What is your name? ");

            string custName = Console.ReadLine();
            if (AuthenticateUser(custName) == true)
            {
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                custName = textInfo.ToTitleCase(custName);
                Console.WriteLine($"\nWelcome {custName}. I hope you are having a good day " +
                    $"{DateTime.Now}\n");
            }
        }

        /// <summary>
        /// Instantiate Computer objects then added to static order list for viewing.
        /// </summary>
        /// <exception cref="ArgumentException">Catches ints that are not cast to double</exception>
        public static void FillOrders()
        {
            try
            {
                orders.Add(new Computer("Laptop", 500));
                orders.Add(new Computer("Laptop", 800, 1024));
                orders.Add(new Computer("Laptop", 900, 1024, (double)1));
                orders.Add(new Computer("Desktop", 1300, 1024, 1.0, 8));
                orders.Add(new Computer("Laptop", 900, diskSize: 1.0));
                orders.Add(new Computer("Desktop", 1200, diskSize: 1.0, processorCores: 8));
                orders.Add(new Computer("Desktop", 1000, 1024, processorCores: 8));
                orders.Add(new Computer("Laptop", 800, processorCores: 8));
            }
            catch(InvalidCastException ex)
            {
                throw new ArgumentException("Disk size was not properly cast as a double", ex);
            }
        }

        /// <summary>
        /// Allows employee user to choose between different options.
        /// </summary>
        public static void GivenOptions()
        {
            Console.WriteLine("\nPress 1 to view customized orders." +
                "\nPress 2 to add a customer to the mailing list." +
                "\nPress 3 to create a customized computer quote.");
            string custResponse = Console.ReadLine();
            switch(custResponse)
            {
                case "1":
                    Option1();
                    break;
                case "2":
                    Option2();
                    break;
                case "3":
                    Option3();
                    break;
                default:
                    Environment.Exit(0);
                    break;
            }
        }

        /// <summary>
        /// Option to display list of orders.
        /// </summary>
        /// <exception cref="ArgumentException">Does not allow to have an empty orders list</exception>
        private static void Option1()
        {
            Console.WriteLine("\nRecent customized computer orders include:");
            double totalPrice = 0;
            int count = 0;
            double averagePrice = 0;
            foreach (Computer PC in orders)
            {
                PC.Display();
                totalPrice += PC.Price;
                count++;
            }
            try
            {
                averagePrice = totalPrice / count;
            }
            catch (DivideByZeroException ex)
            {
                throw new ArgumentException("Average not found when dividing by zero", ex.Message);
            }
            finally
            {
                Console.WriteLine($"\nTotal orders price: ${totalPrice}");
                Console.WriteLine($"\nTotal orders average price: $" +
                    $"{String.Format("{0:.##}", averagePrice)}");
            }
        }

        /// <summary>
        /// Option to add Customer object to XML file.
        /// </summary>
        private static void Option2()
        {
            Customer cust = IdentifyCustomer();
            Console.Write("Enter customer's category of interest: ");
            cust.FavCategory = Console.ReadLine().ToUpper();
            string newPath = CreateDirectory(@"\mailinglist");
            AddToXml(newPath, cust);
        }

        /// <summary>
        /// Option to create a customized quote for a customer
        /// </summary>
        private static void Option3()
        {
            List<Computer> computers= new List<Computer>();
            Customer cust = IdentifyCustomer();
            Console.WriteLine("Enter three of the customer's orders...");
            int count = 0;
            while (count < 3)
            {
                string compOrder = "";
                switch(count)
                {
                    case 0:
                        compOrder = "first";
                        break;
                    case 1:
                        compOrder = "second";
                        break;
                    case 2:
                        compOrder = "third";
                        break;
                }
                Console.Write($"\nEnter the {compOrder} computer type: ");
                string ComputerType = Console.ReadLine().ToUpper();
                Console.Write($"Enter the {compOrder} computer brand: ");
                string Brand = Console.ReadLine().ToUpper();
                Console.Write($"Enter the {compOrder} computer RAM size: ");
                int Ram = Convert.ToInt32(Console.ReadLine());
                Console.Write($"Enter the {compOrder} computer disk size: ");
                double DiskSize = Convert.ToDouble(Console.ReadLine());
                Console.Write($"Enter the {compOrder} computer price: $");
                double Price = Convert.ToDouble(Console.ReadLine());
                computers.Add(new Computer(ComputerType, Brand, Price, Ram, DiskSize));
                count++;
            }
            string newPath = CreateDirectory(@"\quotes");
            double custQuote = AddToExcel(newPath, cust, computers);
            Console.WriteLine("Would you like to view the order quote? (y/n)");
            string answer = Console.ReadLine();
            switch(answer)
            {
                case "y":
                case "Y":
                    Console.WriteLine($"The total of that order is ${custQuote}");
                    break;
                default:
                    Environment.Exit(0);
                    break;
            }
        }

        /// <summary>
        /// Prompts user for customer information
        /// </summary>
        /// <returns>Customer object</returns>
        private static Customer IdentifyCustomer()
        {
            Regex EmailRegEx = new Regex("^[A-Za-z0-9]{3,20}@[A-Za-z]{3,10}\\.(com|net)$");
            Regex PhoneRegEx = new Regex("^\\(?[1-9]{1}[0-9]{2}\\)? ?-?[1-9]{3}-[0-9]{4}$");
            Customer cust = new Customer();
            Console.Write("Enter customer's name: ");
            cust.Name = Console.ReadLine().ToUpper();
            Console.Write("Enter customer's address: ");
            cust.Address = Console.ReadLine().ToUpper();
            Console.Write("Enter customer's e-mail: ");
            bool isEmail;
            do
            {
                string email = Console.ReadLine();
                isEmail = EmailRegEx.IsMatch(email);
                if (isEmail == false)
                {
                    Console.WriteLine("Invalid entry. Enter email with '.com' or '.net' TLD.\nTry again:");
                }
                else { cust.Email = email.ToUpper(); }
            } while (isEmail == false); 
            Console.Write("Enter customer's phone number: ");
            bool isPhone;
            do
            {
                string phone = Console.ReadLine();
                isPhone = PhoneRegEx.IsMatch(phone);
                if (isPhone == false)
                {
                    Console.WriteLine("Invalid entry. US phone format only.\nTry again:");
                }
                else { cust.Phone = phone.ToUpper(); }
            } while (isPhone == false);
            return cust;
        }

        /// <summary>
        /// Creates directory within project to hold XML file.
        /// </summary>
        /// <returns></returns>
        private static string CreateDirectory(string newDirectory)
        {
            string currentPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string newPath = currentPath + newDirectory;
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            return newPath;
        }

        /// <summary>
        /// Uses XDocument class to serialize Customer object and store into XML file
        /// </summary>
        /// <param name="givenPath"></param>
        /// <param name="customer"></param>
        private static void AddToXml(string givenPath, Customer customer)
        {
            string fullPath = givenPath + @"\category.xml";
            if (!File.Exists(fullPath))
            {
                XDocument categoryFile = new XDocument();
                categoryFile.Add(new XElement("Customers"));
                XElement xElement = new XElement("Customer",
                        new XElement("Name", customer.Name),
                        new XElement("Address", customer.Address),
                        new XElement("Email", customer.Email),
                        new XElement("Phone", customer.Phone),
                        new XElement("FavCategory", customer.FavCategory)
                    );
                categoryFile.Root.Add(xElement);
                categoryFile.Save(fullPath);
            }
            else
            {
                XDocument categoryFile = XDocument.Load(fullPath);
                XElement xElement = new XElement("Customer",
                        new XElement("Name", customer.Name),
                        new XElement("Address", customer.Address),
                        new XElement("Email", customer.Email),
                        new XElement("Phone", customer.Phone),
                        new XElement("FavCategory", customer.FavCategory)
                    );
                categoryFile.Root.Add(xElement);
                categoryFile.Save(fullPath);
            }
        }

        /// <summary>
        /// Uses Com Interop to write customized quotes to an excel file
        /// </summary>
        /// <param name="givenPath"></param>
        /// <param name="cust"></param>
        /// <param name="computers"></param>
        /// <returns></returns>
        private static double AddToExcel(string givenPath, Customer cust, List<Computer> computers)
        {
            string fullPath = givenPath + @"\customerQuotes.xlsx";
            Excel.Application excel = new Excel.Application();
            excel.Visible = false;
            double totalPrice = 0;
            if (!File.Exists(fullPath))
            {
                Excel.Workbook workbook = excel.Workbooks.Add();
                Excel._Worksheet workSheet = (Excel.Worksheet)excel.ActiveSheet;

                HeaderExcel(workSheet);
                int row = 2;
                foreach (var comp in computers)
                {
                    for (int c = 1; c <= 9; c++)
                    {
                        workSheet.Cells[row, c] = FillRow(c, cust, comp);
                        workSheet.Columns[c].AutoFit();
                    }
                    totalPrice += comp.Price;
                    row++;
                }
                workSheet.Cells[row, 10] = totalPrice;
                workbook.SaveAs(fullPath);
                workbook.Close();
            }
            else
            {
                Excel.Workbook workbook = excel.Workbooks.Open(fullPath);
                Excel._Worksheet workSheet = (Excel.Worksheet)excel.ActiveSheet;
                Excel.Range lastRow = workSheet.Cells.SpecialCells(
                    Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                int lastRowUsed = lastRow.Row;
                int row = lastRowUsed + 1;
                foreach (var comp in computers)
                {
                    for (int c = 1; c <= 9; c++)
                    {
                        workSheet.Cells[row, c] = FillRow(c, cust, comp);
                        workSheet.Columns[c].AutoFit();
                    }
                    totalPrice += comp.Price;
                    row++;
                }
                workSheet.Cells[row, 10] = totalPrice;
                workbook.Save();
                workbook.Close();
            }
            Console.WriteLine($"Information was added to spreadsheet at {fullPath}");
            return totalPrice;
        }

        /// <summary>
        /// Decides which value to insert in excel cell depending on column
        /// </summary>
        /// <param name="column"></param>
        /// <param name="cust"></param>
        /// <param name="comp"></param>
        /// <returns>Excell cell value string</returns>
        private static dynamic FillRow(int column, Customer cust, Computer comp)
        {
            dynamic ColumnVal = null;
            switch (column)
            {
                case 1:
                    ColumnVal = cust.Name;
                    break;
                case 2:
                    ColumnVal = cust.Address;
                    break;
                case 3:
                    ColumnVal = cust.Email;
                    break;
                case 4:
                    ColumnVal = cust.Phone;
                    break;
                case 5:
                    ColumnVal = comp.ComputerType;
                    break;
                case 6:
                    ColumnVal = comp.Brand;
                    break;
                case 7:
                    ColumnVal = comp.Ram.ToString();
                    break;
                case 8:
                    ColumnVal = comp.DiskSize.ToString();
                    break;
                case 9:
                    ColumnVal = comp.Price.ToString();
                    break;
            }
            return ColumnVal;
        }

        /// <summary>
        /// Creates header columns for new excel sheet
        /// </summary>
        /// <param name="workSheet"></param>
        private static void HeaderExcel(Excel._Worksheet workSheet)
        {
            workSheet.Cells[1, 1] = "CUSTOMER";
            workSheet.Cells[1, 1].Font.FontStyle = "Bold";
            workSheet.Cells[1, 2] = "ADDRESS";
            workSheet.Cells[1, 2].Font.FontStyle = "Bold";
            workSheet.Cells[1, 3] = "EMAIL";
            workSheet.Cells[1, 3].Font.FontStyle = "Bold";
            workSheet.Cells[1, 4] = "PHONE";
            workSheet.Cells[1, 4].Font.FontStyle = "Bold";
            workSheet.Cells[1, 5] = "COMPUTER TYPE";
            workSheet.Cells[1, 5].Font.FontStyle = "Bold";
            workSheet.Cells[1, 6] = "BRAND";
            workSheet.Cells[1, 6].Font.FontStyle = "Bold";
            workSheet.Cells[1, 7] = "RAM";
            workSheet.Cells[1, 7].Font.FontStyle = "Bold";
            workSheet.Cells[1, 8] = "DISK SIZE";
            workSheet.Cells[1, 8].Font.FontStyle = "Bold";
            workSheet.Cells[1, 9] = "PRICE";
            workSheet.Cells[1, 9].Font.FontStyle = "Bold";
            workSheet.Cells[1, 10] = "TOTAL PRICE";
            workSheet.Cells[1, 10].Font.FontStyle = "Bold";
        }

        /// <summary>
        /// Login method to authenticate employee within Introduction() method
        /// </summary>
        /// <param name="userName">Employee's name</param>
        /// <returns>Bool on whether authentication worked</returns>
        private static bool AuthenticateUser(string userName)
        {
            bool result = false;
            int trigger = 0;
            string fullPath = CreateDirectory(@"\loginCredentials") + @"\users.xml";
            if (!File.Exists(fullPath))
            {
                Console.WriteLine("That name was not found!" +
                    "\nEnter a new password to create credentials:");
                string hashPswrd = HashPassword(Console.ReadLine());
                XDocument loginFile = new XDocument();
                loginFile.Add(new XElement("Users"));
                XElement element = new XElement("User",
                    new XElement("Username", userName),
                    new XElement("Password", hashPswrd));
                loginFile.Root.Add(element);
                loginFile.Save(fullPath);
                Console.WriteLine("New credentials created." +
                    "\nAttempt to login with new credentials.");
                Console.ReadLine();
                Environment.Exit(0);
            }
            else
            {
                XDocument loginFile = XDocument.Load(fullPath);
                foreach (XElement user in loginFile.Descendants("User"))
                {
                    if (user.XPathSelectElement("Username").Value == userName)
                    {
                        trigger += 1;
                        Console.WriteLine("Enter your password:");
                        string savedPswrd = user.XPathSelectElement("Password").Value;
                        if (savedPswrd == HashPassword(Console.ReadLine()))
                        {
                            result = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Wrong password");
                            Console.ReadLine();
                            Environment.Exit(0);
                        }
                    }
                }
                if (trigger == 0)
                {
                    Console.WriteLine("That name was not found!" +
                        "\nEnter a new password to create credentials:");
                    string hashPswrd = HashPassword(Console.ReadLine());
                    XElement element = new XElement("User",
                        new XElement("Username", userName),
                        new XElement("Password", hashPswrd));
                    loginFile.Root.Add(element);
                    loginFile.Save(fullPath);
                    Console.WriteLine("New credentials created." +
                        "\nAttempt to login with new credentials.");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
            return result;
        }

        /// <summary>
        /// Hashes given password that cannot be reverse hashed
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Hash value</returns>
        private static string HashPassword(string password)
        {
            var sha = new SHA256Managed();
            byte[] pswdByte = Encoding.UTF8.GetBytes(password);
            return BitConverter.ToString(sha.ComputeHash(pswdByte));
        }
    }
}
// https://uibakery.io/regex-library/phone-number-csharp
// https://www.youtube.com/watch?v=9178EdR6Pyg