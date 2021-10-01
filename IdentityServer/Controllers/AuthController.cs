using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });

        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = new IdentityUser(vm.UserName);
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Redirect(vm.ReturnUrl);
            }
            

            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {

            var result = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, false, false);
            if(result.Succeeded)
            {
                return Redirect(vm.ReturnUrl);
            }
            else if(result.IsLockedOut)
            {

            }

            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
