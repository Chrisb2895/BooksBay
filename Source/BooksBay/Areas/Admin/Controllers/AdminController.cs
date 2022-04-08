using BooksBay.ViewModels;
using LibraryManager.Classes.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace BooksBay.Areas.Admin.Controllers
{
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
