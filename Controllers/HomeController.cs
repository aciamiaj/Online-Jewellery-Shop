using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineJewelleryShop.Models;
using System.Diagnostics;

namespace OnlineJewelleryShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        // Returns the home page view
        public IActionResult Index()
        {
            return View();
        }

        // Returns the privacy policy view, only accessible to authorized users
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        // Returns the error view, with the ID of the current request or a new identifier
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}