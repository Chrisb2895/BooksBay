using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;


namespace LibraryManager.Classes.Controllers
{

    public class CommonController : Controller
    {
        public readonly ILogger<CommonController> _logger;
        public readonly IHttpClientFactory _httpClientFactory;
        public readonly string _API_Endpoint;

        public CommonController(ILogger<CommonController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _API_Endpoint = configuration.GetValue<string>("WebAPI_Endpoint");
        }

        public string GetLoggedUser()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var loggedUser = claims.FirstOrDefault(cl => cl.Type == "name").Value;
            return loggedUser;
        }
    }
}
