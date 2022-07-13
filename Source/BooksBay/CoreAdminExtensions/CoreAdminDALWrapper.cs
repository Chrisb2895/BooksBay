using BooksBay.Common;
using DAL.CoreAdminExtensions;
using Newtonsoft.Json;

namespace BooksBay.CoreAdminExtensions
{
    public class CoreAdminDALWrapper : CommonAPIWrapper
    {
        public readonly ILogger<CoreAdminDALWrapper> _logger;


        public CoreAdminDALWrapper(ILogger<CoreAdminDALWrapper> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
            : base(httpClientFactory, configuration)
        {
            _logger = logger;

        }

        public void GetEntities(IServiceCollection services)
        {
            var coreAdminOptions = new CoreAdminOptions();
            var discoveredServices = new List<DiscoveredDbSetEntityType>();
            discoveredServices = GetAsync<IEnumerable<DiscoveredDbSetEntityType>>("api/DatabaseGeneric").Result.ToList();

            var servicesToRemove = services.Where(s => s.ServiceType == typeof(DiscoveredDbSetEntityType)).ToList();
            foreach (var serviceToRemove in servicesToRemove)
            {
                services.Remove(serviceToRemove);
            }


            foreach (var service in discoveredServices)
            {
                services.AddTransient(_ => service);
            }

            services.AddSingleton(coreAdminOptions);
        }
    }
}
