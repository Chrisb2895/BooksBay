using Microsoft.EntityFrameworkCore.Metadata;

namespace LOGIC.Services.Models.CoreAdminDataModels
{
    public class CoreAdminDataIndex
    {
        public IQueryable<object> Query { get; set; }

        public IEnumerable<INavigation> Navigations { get; set; }

        public IReadOnlyList<IProperty> Properties { get; set; }
    }
}
