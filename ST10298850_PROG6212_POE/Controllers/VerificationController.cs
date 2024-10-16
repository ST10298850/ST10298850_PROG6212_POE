using Microsoft.AspNetCore.Mvc;

namespace ST10298850_PROG6212_POE.Controllers
{
    public class VerificationController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        // New action method to return the VerificationPageView
        public IActionResult VerificationPageView()
        {
            return View();
        }
    }
}
