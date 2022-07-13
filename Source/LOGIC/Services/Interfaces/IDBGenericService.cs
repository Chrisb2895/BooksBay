using DAL.DataContext;
using LOGIC.Services.Models;
using LOGIC.Services.Models.CoreAdminDataModels;

namespace LOGIC.Services.Interfaces
{
    public interface IDBGenericService
    {
        public GenericResultSet<DatabaseContext> GetDBContext();

        //public GenericResultSet<IQueryable<object>> Set(Type type);

        public GenericResultSet<CoreAdminDataIndex> CoreAdminDataIndex(Type type);



    }
}
