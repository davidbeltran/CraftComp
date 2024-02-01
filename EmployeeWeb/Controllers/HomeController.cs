using EmployeeWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Security.Cryptography;

namespace EmployeeWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View();
        }

        public IActionResult Locations()
        {
            return View();
        }

        public IActionResult MailingList()
        {
            return View();
        }

        public IActionResult Quotes()
        {
            return View();
        }

        /// <summary>
        /// Allows excel sheet to be downloaded
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Download()
        {
            byte[] fileByteArray = System.IO.File.ReadAllBytes(Program.QuoteExcel);
            return File(fileByteArray, "application/vnd.ms-excel", "customerQuotes.xlsx");
        }

        public IActionResult Users()
        {
            return View();
        }

        public IActionResult LoginError()
        {
            return View();
        }

        public IActionResult LoginSuccess()
        {
            return View();
        }

        /// <summary>
        /// Authenticates user for accessing link page.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IActionResult Login(string username, string password)
        {
            int trigger = 0;
            password = HashPassword(password);
            string currentPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            currentPath = Path.GetFullPath(Path.Combine(currentPath, @"Craft_A_Comp\CompStoreConsole\loginCredentials\users.xml"));
            if (!System.IO.File.Exists(currentPath))
            {
                return LocalRedirect("/Home/LoginError");
            }
            else
            {
                XDocument loginFile = XDocument.Load(currentPath);
                foreach (XElement user in loginFile.Descendants("User"))
                {
                    if ((user.XPathSelectElement("Username").Value == username) &&
                        (user.XPathSelectElement("Password").Value == password))
                    {
                        trigger++;
                    }
                }
                if (trigger == 0)
                {
                    return LocalRedirect("/Home/LoginError");
                }
                else
                {
                    return LocalRedirect("/Home/LoginSuccess");
                }
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        /// <summary>
        /// Private method to hash user login password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private static string HashPassword(string password)
        {
            if (password == null) 
            {
                password = "";
            }
            var sha = new SHA256Managed();
            byte[] pswdByte = Encoding.UTF8.GetBytes(password);
            return BitConverter.ToString(sha.ComputeHash(pswdByte));
        }
    }
}