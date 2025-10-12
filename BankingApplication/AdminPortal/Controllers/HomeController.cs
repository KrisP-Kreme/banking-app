using System.Diagnostics;
using AdminPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Controllers
{
    public class HomeController : Controller
    {
        // ensure that the only valid login details are the following
        private const string VALID_USERNAME = "admin";
        private const string VALID_PASSWORD = "admin";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // if logged in, go to payee page, else go into login page
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                return RedirectToAction("Index", "Payees");
            }
            else
            {
                return RedirectToAction("Login");
            }            
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            // Check credentials
            if (username == VALID_USERNAME && password == VALID_PASSWORD)
            {
                HttpContext.Session.SetString("Username", username);
                return RedirectToAction("Index", "Payees");
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
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
