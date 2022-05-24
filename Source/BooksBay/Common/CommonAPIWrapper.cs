using Newtonsoft.Json;

namespace BooksBay.Common
{
    public class CommonAPIWrapper
    {
        public readonly IHttpClientFactory _httpClientFactory;
        public readonly string _API_Endpoint;

        public CommonAPIWrapper( IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {

            _httpClientFactory = httpClientFactory;
            _API_Endpoint = configuration.GetValue<string>("WebAPI_Endpoint");
        }

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
    }
}
