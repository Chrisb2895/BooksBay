using DAL.DataContext;
using LOGIC.Services.Models;

namespace LOGIC.Services.Interfaces
{
    public interface IDBGenericService
    {
        public GenericResultSet<DatabaseContext> GetDBContext();

        public GenericResultSet<IQueryable<object>> Set(Type type);



    }
}
