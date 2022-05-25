using LOGIC.Services.Models;

namespace LOGIC.Services.Interfaces
{
    public interface IDBGenericService
    {
        public GenericResultSet<IQueryable<object>> Set(Type type);

    }
}
