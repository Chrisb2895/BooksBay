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
    
    public class AccountController : Controller
    {

        LibraryAPI _api = new LibraryAPI();

        [HttpGet]
        [Route("Account/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Account/Register")]
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
                var result = await _api.RegisterUser(mod);
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }

                if (result is IdentityResultDTO)
                {
                    foreach (IdentityError error in ((IdentityResultDTO)result).Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
            }
            return View(model);
        }
    }
}
