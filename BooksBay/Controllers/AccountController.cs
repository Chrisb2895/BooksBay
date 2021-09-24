using BooksBay.Helpers;
using BooksBay.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API = LibraryManager.Controllers;
using APIModels = LibraryManager.Models;

namespace BooksBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {

        LibraryAPI _api = new LibraryAPI();

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("account/Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                APIModels.RegisterViewModel mod = new APIModels.RegisterViewModel()
                {
                    Email = model.Email,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword
                };
                var result = _api.RegisterUser(mod);
                if (result.IsCompletedSuccessfully)
                {
                    return RedirectToAction("index", "home");
                }

                if (result.Result is IdentityResult)
                {
                    foreach (IdentityError error in ((IdentityResult)result.Result).Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
            }
            return View(model);
        }
    }
}
