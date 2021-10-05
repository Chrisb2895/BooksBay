using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace LibraryManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /*[Route("/home")]
        public async Task<IActionResult>  Index()
        {
            //retrieve access token
            var serverClient = _httpClientFactory.CreateClient();

            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44326/");

            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest {
            
            
                Address = discoveryDocument.TokenEndpoint,

                ClientId = "client_id",
                ClientSecret = "client_secret",
                Scope = "ApiOne"

            });
            //retrieve access data
            var apiClient = _httpClientFactory.CreateClient();

            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("https://localhost:44380/secret");

            var content = await response.Content.ReadAsStringAsync();

            return Ok(new { 
            
            access_token = tokenResponse.AccessToken,
            message = content
            });
        }*/
    }
}
