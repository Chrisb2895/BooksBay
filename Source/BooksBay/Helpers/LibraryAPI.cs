using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using APIModels = LibraryManager.Models;

namespace BooksBay.Helpers
{
    public class LibraryAPI
    {
        private readonly string _API_Endpoint;

        public LibraryAPI(IConfiguration configuration)
        {
            _API_Endpoint = configuration.GetValue<string>("WebAPI_Endpoint");
        }

        private HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_API_Endpoint);
            return client;
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
