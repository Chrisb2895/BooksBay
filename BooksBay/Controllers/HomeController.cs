using BooksBay.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BooksBay.Controllers
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

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        [Authorize]
        public IActionResult Authenticate()
        {
            var userIDClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob"),
                new Claim(ClaimTypes.Email,"Bob@test.it")
            };

            var userDriverLicenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob Jr"),
                new Claim("Driving License Type","B")
            };

            var userIDCardClaimIdentity = new ClaimsIdentity(userIDClaims, "claim identity card");
            var userDLicenseClaimIdentity = new ClaimsIdentity(userDriverLicenseClaims, "claim driving license");

            var userPrincipal = new ClaimsPrincipal(new[] { userIDCardClaimIdentity,userDLicenseClaimIdentity });

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("index");
        }

        public IActionResult Logout()
        {
            return SignOut("Cookie","oidc");
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
