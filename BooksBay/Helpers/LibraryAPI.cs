using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using APIModels = LibraryManager.Models;

namespace BooksBay.Helpers
{
    public class LibraryAPI
    {

        public HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44380/");
            return client;
        }

        public async Task<IActionResult> RegisterUser(APIModels.RegisterViewModel model)
        {
            Task<IActionResult> result;
            HttpClient cli = this.Initial();
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage res = await cli.PostAsync("api/account/Register", stringContent);
            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<Task<IActionResult>>(results);
                return result.Result;

            }
            return new ObjectResult(new { error = res.Content.ReadAsStringAsync().Result})
            {
                StatusCode = (int)res.StatusCode
            };
        }
    }
}
