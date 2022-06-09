using IdentityServer4.Services;
using LibraryManager.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace LibraryManager.Controllers
{
    //This controller is used mvc like because ASP Net Identity ClaimsIdentity class is not completly serializable and so cant use API like.....
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;
        public readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IIdentityServerInteractionService interactionService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interactionService = interactionService;
            _configuration = configuration;
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
                    return LocalRedirect(vm.ReturnUrl);
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

            //SPEED UP LOGIN WHILE DEBUGGING - TOREMOVE TO PRD NOTES
            if (System.Diagnostics.Debugger.IsAttached)
            {
                return Login(new LoginViewModel()
                {
                    Email = _configuration["LoginUser"],
                    Password = _configuration["LoginPwd"],
                    ExternalProviders = externalProviders,
                    RememberMe = false,
                    ReturnUrl = returnUrl
                }).Result;
                
            }


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
                    return LocalRedirect(vm.ReturnUrl);
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

            return LocalRedirect(logoutRequest.PostLogoutRedirectUri);

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
            //Security check against open redirects threats
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = "";

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction("Login");

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, false);

            if (result.Succeeded)
            {
                return View("LoginResult", new LoginResultViewModel(true, returnUrl));
            }

            if (result.RequiresTwoFactor)
            {
                bool hasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null;

                if (!hasAuthenticator)
                    return RedirectToPage("./EnableAuthenticator", new { ReturnUrl = returnUrl, RememberMe = false });

                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = false });
            }

            var userName = info.Principal.FindFirst(ClaimTypes.Name.Replace(" ", "_")).Value;

            return View("LoginResult", new LoginResultViewModel(false, Url.Action("ExternalRegister")));

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

                    return LocalRedirect(vm.ReturnUrl);

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
