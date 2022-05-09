using BooksBay.Controllers;
using BooksBay.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace BooksBay.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : CommonController
    {

        public AdminController(ILogger<CommonController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(logger, httpClientFactory, configuration)
        {

        }

        [Authorize]
        public IActionResult Index()
        {
            var loggedUser = GetLoggedUser();
            return View(new BaseViewModel { CurrentLoggedUser = loggedUser });
        }
    }
}
