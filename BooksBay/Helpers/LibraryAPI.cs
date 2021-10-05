using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using APIModels = LibraryManager.Models;

namespace BooksBay.Helpers
{
    public class LibraryAPI
    {

        private HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44380/");
            return client;
        }

        public async Task<IdentityResultDTO> RegisterUser(APIModels.RegisterViewModel model)
        {
            
            IdentityResultDTO result = null;
            /*HttpClient cli = this.Initial();
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage res = await cli.PostAsync("api/Account/RegisterUser", stringContent);
            if (res.IsSuccessStatusCode)
            {
                var results = await  res.Content.ReadAsStringAsync();
                realRes = JsonConvert.DeserializeObject<IdentityResultDTO>(results);
                return realRes;

            }*/

            result = await PostAsync<IdentityResultDTO>("api/Account/RegisterUser", model);

            return result;
        }

        public async Task<bool> IsSignedIn( System.Security.Claims.ClaimsPrincipal user)
        {
            bool result = false;
            ClaimsPrincipal us = (ClaimsPrincipal)user;

            result = await PostAsync<bool>("api/Account/User/IsSignedIn", user);
           
            return result;
        }

        public async Task SignOut()
        {

            await PostAsync<bool>("api/Account/User/SignOut", null);

        }

        private async Task<TResult> PostAsync<TResult>(string requestUri,object postData)
        {
            HttpClient cli = this.Initial();
            var stringContent = new StringContent(JsonConvert.SerializeObject(postData), UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage res = await cli.PostAsync(requestUri, stringContent);
            if (res.IsSuccessStatusCode)
            {
                var results = await res.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(results);
               
            }

            return default(TResult);
        }
    }
}
