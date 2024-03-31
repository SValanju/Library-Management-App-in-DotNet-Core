using Library_WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Library_WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Dashboard()
        {
            string userRole = User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.Role)?.Value;

            List<Tuple<string, string, string, string>> modules = new List<Tuple<string, string, string, string>>();
            if(!string.IsNullOrEmpty(userRole))
            {
                Tuple<string, string, string, string> booksModule = new Tuple<string, string, string, string>("lib.png", "BooksListing", "Books", "card-img-top w-75");
                Tuple<string, string, string, string> usersModule = new Tuple<string, string, string, string>("users-management.png", "UsersListing", "User", "card-img-top");

                switch (userRole)
                {
                    case "ADMIN":
                        modules.Add(booksModule);
                        modules.Add(usersModule);
                        break;
                    case "TEACHER":
                        modules.Add(booksModule);
                        break;
                }
            }

			return View(modules);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied([FromQuery] string returnUrl)
        {
			if (!string.IsNullOrWhiteSpace(returnUrl))
				ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
