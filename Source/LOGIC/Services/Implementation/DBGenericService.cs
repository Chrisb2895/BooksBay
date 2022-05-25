using DAL;
using DAL.DataContext;
using DAL.Functions.CRUD;
using LOGIC.Services.Interfaces;
using LOGIC.Services.Models;
using Microsoft.EntityFrameworkCore;


namespace LOGIC.Services.Implementation
{
    public class DBGenericService : IDBGenericService
    {
        private ICRUD _CRUD;
        private DatabaseContext _dbContext;

        public DBGenericService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
            _CRUD = new CRUD(_dbContext);

        }

        public GenericResultSet<IQueryable<object>> Set(Type type)
        {
            GenericResultSet<IQueryable<object>> result = new GenericResultSet<IQueryable<object>>();
            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            var fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
            try
            {
                IQueryable<object> res = _CRUD.Set(type);

                result.UserMessage = $"The supplied type {type.Name} was Set successfully ";
                result.InternalMessage = $"{fullName} executed successfully";
                result.ResultSet = res;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.UserMessage = $"There was an error Setting type {type.Name} to get IQueryable object, please try again";
                result.InternalMessage = $"ERROR: {fullName} : {ex.Message}";
            }
            return result;
        }
    }
}
