using BooksBay.Areas.Admin.ViewModels;
using DAL.CoreAdminExtensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BooksBay.Areas.Admin.ViewComponents
{
    public class CoreAdminMenuViewComponent : ViewComponent
    {
        private readonly IEnumerable<DiscoveredDbSetEntityType> dbSetEntities;
        public readonly ILogger<CoreAdminMenuViewComponent> _logger;


        public CoreAdminMenuViewComponent(ILogger<CoreAdminMenuViewComponent> logger, IEnumerable<DiscoveredDbSetEntityType> dbSetEntities)
        {
            _logger = logger;
            this.dbSetEntities = dbSetEntities;
        }


        public IViewComponentResult Invoke()
        {

            var viewModel = new MenuViewModel();
            foreach (var dbSetEntity in dbSetEntities)
            {
                viewModel.DbContextNames.Add(dbSetEntity.DbContextType.Name);
                viewModel.DbSetNames.Add(dbSetEntity.Name);
            }

            return View(viewModel);
        }
    }
}
