﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            var serverClient = _httpClientFactory.CreateClient();
            var getDbResponse = serverClient.GetAsync(_API_Endpoint + methodName);
            if (getDbResponse.Result.IsSuccessStatusCode)
            {
                var jsonDbResult = await getDbResponse.Result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonDbResult);
            }
            return default(T);
        }

        [NonAction]
        public async Task<TResult> PostAsync<TResult>(string methodName, object postData)
        {
            var serverClient = _httpClientFactory.CreateClient();
            var stringContent = new StringContent(JsonConvert.SerializeObject(postData), UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage res = await serverClient.PostAsync(_API_Endpoint + methodName, stringContent);
            if (res.IsSuccessStatusCode)
            {
                var results = await res.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(results);

            }

            return default(TResult);
        }
    }
}
