using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BooksBay.Common
{
    public class CoreAdminCommonController : Controller
    {

        public readonly ILogger<CoreAdminCommonController> _logger;
        public readonly IHttpClientFactory _httpClientFactory;
        public readonly string _API_Endpoint;

        public CoreAdminCommonController(ILogger<CoreAdminCommonController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _API_Endpoint = configuration.GetValue<string>("WebAPI_Endpoint");
        }

        public IActionResult Index()
        {
            return View();
        }

        [NonAction]
        public async Task<T> GetAsync<T>(string methodName)
        {
            try
            {
                var serverClient = _httpClientFactory.CreateClient();
                var getDbResponse = serverClient.GetAsync(_API_Endpoint + methodName);
                if (getDbResponse.Result.IsSuccessStatusCode)
                {
                    var jsonDbResult = await getDbResponse.Result.Content.ReadAsStringAsync();
                    var res = JsonConvert.DeserializeObject<T>(jsonDbResult);
                    return res;
                }
                return default(T);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "CoreAdminCommonController Get Error : {0} {1}", ex.Message, ex.StackTrace);
                return default(T);
            }
            
        }

        [NonAction]
        public async Task<TResult> PostAsync<TResult>(string methodName, object postData)
        {
            var serverClient = _httpClientFactory.CreateClient();
            var stringContent = new StringContent(JsonConvert.SerializeObject(postData), UnicodeEncoding.UTF8, "application/json");
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage res = await serverClient.PostAsync(_API_Endpoint + methodName, stringContent);
            var results = await res.Content.ReadAsStringAsync();
            if (res.IsSuccessStatusCode)
            {
                var ret = JsonConvert.DeserializeObject<TResult>(results, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore,

                });
                return ret;

            }

            return default(TResult);
        }
    }
}
