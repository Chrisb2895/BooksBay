using DAL.CoreAdminExtensions;
using Newtonsoft.Json;

namespace BooksBay.CoreAdminExtensions
{
    public class CoreAdminDALWrapper
    {
        public readonly ILogger<CoreAdminDALWrapper> _logger;
        public readonly IHttpClientFactory _httpClientFactory;
        public readonly string _API_Endpoint;


        public CoreAdminDALWrapper(ILogger<CoreAdminDALWrapper> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _API_Endpoint = configuration.GetValue<string>("WebAPI_Endpoint");
        }

        public void GetEntities(IServiceCollection services)
        {
            var serverClient = _httpClientFactory.CreateClient();
            var getDbResponse = serverClient.GetAsync(_API_Endpoint + "api/DatabaseGeneric");
            var discoveredServices = new List<DiscoveredDbSetEntityType>();
            if (getDbResponse.Result.IsSuccessStatusCode)
            {
                var jsonDbResult = getDbResponse.Result.Content.ReadAsStringAsync();
                discoveredServices =  JsonConvert.DeserializeObject<IEnumerable<DiscoveredDbSetEntityType>>(jsonDbResult.Result).ToList();
            }
            var servicesToRemove = services.Where(s => s.ServiceType == typeof(DiscoveredDbSetEntityType)).ToList();
            foreach (var serviceToRemove in servicesToRemove)
            {
                services.Remove(serviceToRemove);
            }

                    
            foreach (var service in discoveredServices)
            {
                services.AddTransient(_ => service);
            }
        }
    }
}
