using BooksBay.Areas.Admin.ViewModels;
using DAL.CoreAdminExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BooksBay.Areas.Admin.ViewComponents
{
    public class CoreAdminMenuViewComponent : ViewComponent
    {
        private readonly IEnumerable<DiscoveredDbSetEntityType> dbSetEntities;

        public CoreAdminMenuViewComponent(IEnumerable<DiscoveredDbSetEntityType> dbContexts)
        {
            this.dbSetEntities = dbContexts;
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
