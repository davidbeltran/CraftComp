/*
    Substance Author: David Beltran
    Contains .cs code that controls buttons in View/Home folder.
 */
using CompStoreWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace CompStoreWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// IActionResult return methods operate buttons on web pages.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Laptops()
        {
            return View();
        }

        public IActionResult Tablets()
        {
            return View();
        }

        public IActionResult Accessories()
        {
            return View();
        }

        /// <summary>
        /// Method to display list of location cities to .cshtml
        /// </summary>
        /// <returns></returns>
        public IActionResult Locations()
        {
            var locationsData = Program.locations;

            var model = new LocationViewModel();
            model.LocationsSelectList = new List<SelectListItem>();

            foreach (var location in locationsData)
            {
                model.LocationsSelectList.Add(new SelectListItem { Text = location.City, Value = location.GetInfo() });
            }

            return View(model);
        }

        /// <summary>
        /// Method to provide event of button to display entire location information
        /// to .cshtml
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Locations(LocationViewModel model)
        {
            return View(model);
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
    }
}