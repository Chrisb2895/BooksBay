using Microsoft.EntityFrameworkCore.Metadata;

namespace LOGIC.Services.Models.CoreAdminDataModels
{
    public class CoreAdminDataIndex
    {
        public List<object> Query { get; set; }

        public List<INavigation> Navigations { get; set; }

        //public List<IProperty> Properties { get; set; }
    }
}
