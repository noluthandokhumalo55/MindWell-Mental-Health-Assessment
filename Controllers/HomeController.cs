using Microsoft.AspNetCore.Mvc;
using Mindwell.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Mindwell.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Ensure the user is authenticated before accessing this action
        public IActionResult Index()
        {
            // Check if the user is logged in (via session)
            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userEmail))
            {
                // Redirect to login page if not authenticated
                return RedirectToAction("Index", "Login_R");
            }

            return View();
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
