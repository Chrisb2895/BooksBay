using Microsoft.AspNetCore.Identity;
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

        public async Task<IdentityResultDTO> RegisterUser(APIModels.RegisterViewModel model)
        {
            IdentityResult result = null;
            IdentityResultDTO realRes=null;
            HttpClient cli = this.Initial();
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage res = await cli.PostAsync("api/Account/RegisterUser", stringContent);
            if (res.IsSuccessStatusCode)
            {
                var results = await  res.Content.ReadAsStringAsync();
                realRes = JsonConvert.DeserializeObject<IdentityResultDTO>(results);
                result = realRes;
                return realRes;

            }
            return null;
        }
    }
}
