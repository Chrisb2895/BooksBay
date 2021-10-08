using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManager.ViewModels;


namespace LibraryManager.Controllers
{
    //[Route("[controller]")]
    //[System.Web.Http.RoutePrefix("/Auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IIdentityServerInteractionService interactionService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interactionService = interactionService;
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });

        }

        [HttpPost]
        [Route("/Register")]
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
        [Route("/Auth/Login")]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });

        }

        [HttpGet]
        [Route("/Logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);
            if (string.IsNullOrEmpty(logoutRequest.PostLogoutRedirectUri))
                return RedirectToAction("Index", "Home");

            return Redirect(logoutRequest.PostLogoutRedirectUri);

        }

        [HttpPost]
        [Route("/Login")]
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

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
