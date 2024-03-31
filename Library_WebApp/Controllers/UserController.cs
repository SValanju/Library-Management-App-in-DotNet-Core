using DataEncryption;
using Library_WebApp.Models.User;
using Library_WebApp.Services.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace Library_WebApp.Controllers
{
    [Authorize(Policy = "AdminRole")]
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService userService;
        private readonly IConfiguration config;

        public UserController(ILogger<HomeController> logger, IUserService service, IConfiguration configuration)
        {
            _logger = logger;
            userService = service;
            config = configuration;
        }

        [AllowAnonymous]
        public IActionResult LoginPage([FromQuery] string returnUrl)
        {
            ViewBag.AES_Key = new HtmlString(config.GetValue<string>("AES:Key"));
            ViewBag.AES_IV = new HtmlString(config.GetValue<string>("AES:IV"));
            
            if (!string.IsNullOrWhiteSpace(returnUrl))
                TempData["loginErrors"] = "Session expired while accessing page " + returnUrl;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authenticate(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                LoginViewResult loginResult = await userService.GetToken(login);

                if (!string.IsNullOrWhiteSpace(loginResult.token))
                {
                    HttpContext.Session.SetString("loggedInUser", JsonConvert.SerializeObject(loginResult));

                    // Authorization
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Role, loginResult.userDetails.strRole),
                        new Claim(ClaimTypes.Name, loginResult.userDetails.full_name)
                    };
                    var claimIdentity= new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var userPrinciple = new ClaimsPrincipal(claimIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrinciple);

                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    TempData["loginErrors"] = loginResult.errors;
                }
            }
            return RedirectToAction("LoginPage", "User");
        }

        [AllowAnonymous]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("loggedInUser");
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(actionName: "LoginPage", controllerName: "User");
        }

        public IActionResult UsersListing()
        {
            return View();
        }
    }
}
