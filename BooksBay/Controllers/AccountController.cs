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


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                API.AccountController accService = new API.AccountController();
                APIModels.RegisterViewModel mod = new APIModels.RegisterViewModel()
                {
                    Email = model.Email,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword
                };
                var result = accService.RegisterUser(mod);
                if (result.IsCompletedSuccessfully)
                {
                    return RedirectToAction("index", "home");
                }

                if (result is IdentityResult)
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
