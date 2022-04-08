using BooksBay.Models;
using BooksBay.ViewModels;
using IdentityModel.Client;
using LibraryManager.Classes.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BooksBay.Controllers
{
    public class HomeController : CommonController
    {


        public HomeController(ILogger<CommonController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(logger, httpClientFactory, configuration)
        {

        }

        [Authorize]
        public async Task<IActionResult> Index()
        {

            await RefreshAccessToken();
            var identity = (ClaimsIdentity)User.Identity;
            var loggedUser = GetLoggedUser();
            if (identity.IsAuthenticated)
                return View(new BaseViewModel { CurrentLoggedUser = loggedUser });

            return View();
        }

        [HttpPost]
        [Route("Account/Logout")]
        public IActionResult Logout(RegisterViewModel model)
        {
            return SignOut("Cookie", "oidc");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Utils

        private async Task RefreshAccessToken()
        {
            var serverClient = _httpClientFactory.CreateClient();
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync(_API_Endpoint);

            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            var refreshTokenClient = _httpClientFactory.CreateClient();

            var tokenResponse = await refreshTokenClient.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                RefreshToken = refreshToken,
                ClientId = "client_id_mvc",
                ClientSecret = "client_secret_mvc"
            });

            var authInfo = await HttpContext.AuthenticateAsync("Cookie");

            authInfo.Properties.UpdateTokenValue("access_token", tokenResponse.AccessToken);
            authInfo.Properties.UpdateTokenValue("id_token", tokenResponse.IdentityToken);
            authInfo.Properties.UpdateTokenValue("refresh_token", tokenResponse.RefreshToken);

            await HttpContext.SignInAsync("Cookie", authInfo.Principal, authInfo.Properties);

        }



        #endregion
    }
}
