using Microsoft.AspNetCore.Mvc;

namespace ST10298850_PROG6212_POE.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult SignOut()
        {
            HttpContext.Session.Clear(); // Clear all session data
            return RedirectToAction("Index", "Home"); // Redirect to the home page
        }

        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult HRSignOut()
        {
            // Logic for HR sign-out (e.g., redirect to sign-in page)
            return RedirectToAction("Index", "Home");
        }
    }
}