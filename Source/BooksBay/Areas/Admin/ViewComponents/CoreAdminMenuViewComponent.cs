using BooksBay.Areas.Admin.ViewModels;
using DAL.CoreAdminExtensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BooksBay.Areas.Admin.ViewComponents
{
    public class CoreAdminMenuViewComponent : ViewComponent
    {
        private IEnumerable<DiscoveredDbSetEntityType> dbSetEntities;
        public readonly ILogger<CoreAdminMenuViewComponent> _logger;
        public readonly IHttpClientFactory _httpClientFactory;
        public readonly string _API_Endpoint;

        public CoreAdminMenuViewComponent(ILogger<CoreAdminMenuViewComponent> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _API_Endpoint = configuration.GetValue<string>("WebAPI_Endpoint");
        }

        public IViewComponentResult Invoke()
        {
            var serverClient = _httpClientFactory.CreateClient();
            var getDbResponse = serverClient.GetAsync(_API_Endpoint + "api/DatabaseGeneric");
            if (getDbResponse.Result.IsSuccessStatusCode)
            {
                var jsonDbResult = getDbResponse.Result.Content.ReadAsStringAsync();
                this.dbSetEntities = JsonConvert.DeserializeObject<IEnumerable<DiscoveredDbSetEntityType>>(jsonDbResult.Result);
            }
            var viewModel = new MenuViewModel();
            foreach (var dbSetEntity in this.dbSetEntities)
            {
                viewModel.DbContextNames.Add(dbSetEntity.DbContextType.Name);
                viewModel.DbSetNames.Add(dbSetEntity.Name);
            }

            return View(viewModel);
        }
    }
}
