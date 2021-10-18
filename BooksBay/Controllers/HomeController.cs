using BooksBay.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BooksBay.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger,IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            //retrieve access token
            var serverClient = _httpClientFactory.CreateClient();

            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44380/");

            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {


                Address = discoveryDocument.TokenEndpoint,

                ClientId = "client_id_mvc",
                ClientSecret = "client_secret_mvc",
                Scope = "ApiOne"

            });
            //retrieve access data
            var apiClient = _httpClientFactory.CreateClient();

            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("https://localhost:44306/home");

            var content = await response.Content.ReadAsStringAsync();

            await RefreshAccessToken();

            /*return Ok(new
            {

                access_token = tokenResponse.AccessToken,
                message = content
            });*/

            return View();
        }

        public async Task RefreshAccessToken()
        {
            var serverClient = _httpClientFactory.CreateClient();
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44380/");

            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            var refreshTokenClient = _httpClientFactory.CreateClient();

            var tokenResponse = await refreshTokenClient.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                RefreshToken = refreshToken,
                ClientId="client_id_mvc",
                ClientSecret = "client_secret_mvc"
            });

            var authInfo = await HttpContext.AuthenticateAsync("Cookie");

            authInfo.Properties.UpdateTokenValue("access_token", tokenResponse.AccessToken);
            authInfo.Properties.UpdateTokenValue("id_token", tokenResponse.IdentityToken);
            authInfo.Properties.UpdateTokenValue("refresh_token", tokenResponse.RefreshToken);

            await HttpContext.SignInAsync("Cookie", authInfo.Principal, authInfo.Properties);


            /*
            var requestData = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44380/oauth/token")
            {
                Content = new FormUrlEncodedContent(requestData)
            };

            var basicCreds = "username:password";
            var encodedCreds = Encoding.UTF8.GetBytes(basicCreds);
            var base64Creds = Convert.ToBase64String(encodedCreds);

            request.Headers.Add("Authorization", $"Basic {base64Creds}");

            var response = await refreshTokenClient.SendAsync(request);

            var responseString = response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString.Result);

            var newAccessToken = responseData.GetValueOrDefault("access_token");
            var newRefreshToken = responseData.GetValueOrDefault("refresh_token");*/


        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        /*[Authorize]
        public IActionResult Authenticate()
        {
            var userIDClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob"),
                new Claim(ClaimTypes.Email,"Bob@test.it")
            };

            var userDriverLicenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob Jr"),
                new Claim("Driving License Type","B")
            };

            var userIDCardClaimIdentity = new ClaimsIdentity(userIDClaims, "claim identity card");
            var userDLicenseClaimIdentity = new ClaimsIdentity(userDriverLicenseClaims, "claim driving license");

            var userPrincipal = new ClaimsPrincipal(new[] { userIDCardClaimIdentity,userDLicenseClaimIdentity });

            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("index");
        }*/

       
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
