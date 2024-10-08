using Microsoft.AspNetCore.Mvc;
using ST10298850_PROG6212_POE.Models;
using System.Diagnostics;

namespace ST10298850_PROG6212_POE.Controllers
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

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ClaimPageView()
        {
            return View();
        }
        public IActionResult VerificationPageView()
        {
            return View();
        }

        public IActionResult ApprovalPageView()
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
