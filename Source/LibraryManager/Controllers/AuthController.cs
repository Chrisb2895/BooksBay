using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManager.ViewModels;
using System.Security.Claims;
using LibraryManager.Classes.Controllers;

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
            if (ModelState.IsValid)
            {

                var user = new IdentityUser(vm.Email);
                var result = await _userManager.CreateAsync(user, vm.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return Redirect(vm.ReturnUrl);
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }


            return View(vm);
        }

        [HttpGet]
        [Route("/Auth/Login")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            
            //External Login Google Step 2
            var externalProviders = await _signInManager.GetExternalAuthenticationSchemesAsync();

            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalProviders = externalProviders

            });

        }

        [HttpPost]
        [Route("/Auth/Login")]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(vm.Email, vm.Password, vm.RememberMe, false);
                if (result.Succeeded)
                {
                    return Redirect(vm.ReturnUrl);
                }
                else if (result.IsLockedOut)
                {
                    //to complete in future
                }
                ModelState.AddModelError("", "Invalid login attempt");
            }

            return View(vm);
        }

        [HttpGet]
        [Route("/Auth/Logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);
            if (string.IsNullOrEmpty(logoutRequest.PostLogoutRedirectUri))
                return RedirectToAction("Index", "Home");

            return Redirect(logoutRequest.PostLogoutRedirectUri);

        }

        //External Login FB Step 4
        [HttpPost]
        [Route("/Auth/ExternalLogin")]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            var redirectUri = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUri);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [Route("/Auth/ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction("Login");

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false,false);

            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }

            if (result.RequiresTwoFactor)
            {
                bool hasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null;

                if(!hasAuthenticator)
                    return RedirectToPage("./EnableAuthenticator", new { ReturnUrl = returnUrl, RememberMe = false });

                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = false });
            }

            var userName = info.Principal.FindFirst(ClaimTypes.Name.Replace(" ", "_")).Value;

            return View("ExternalRegister", new ExternalRegisterViewModel
            {

                Email = userName,
                ReturnUrl = returnUrl

            });
        }

        [HttpPost]
        [Route("/Auth/ExternalRegister")]
        public async Task<IActionResult> ExternalRegister(ExternalRegisterViewModel vm)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction("Login");

            var user = new IdentityUser(vm.Email);

            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                result = await _userManager.AddLoginAsync(user, info);

                if (result.Succeeded)
                {
                     await _signInManager.SignInAsync(user, false);

                    return Redirect(vm.ReturnUrl);

                }
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(vm);
        }



        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
