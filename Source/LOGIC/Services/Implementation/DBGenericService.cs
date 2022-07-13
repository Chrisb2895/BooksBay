using DAL;
using DAL.DataContext;
using DAL.Functions.CRUD;
using LOGIC.Services.Interfaces;
using LOGIC.Services.Models;
using LOGIC.Services.Models.CoreAdminDataModels;

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

        public GenericResultSet<DatabaseContext> GetDBContext()
        {
            GenericResultSet<DatabaseContext> result = new GenericResultSet<DatabaseContext>();
            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            var fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
            try
            {
                result.UserMessage = $"The DataBaseContext was retrieved successfully ";
                result.InternalMessage = $"{fullName} executed successfully";
                result.ResultSet = _dbContext;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.UserMessage = $"There was an error getting DatabaseContext, please try again";
                result.InternalMessage = $"ERROR: {fullName} : {ex.Message}";
            }
            return result;
        }

        /*public GenericResultSet<IQueryable<object>> Set(Type type)
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
        }*/

        public GenericResultSet<CoreAdminDataIndex> CoreAdminDataIndex(Type type)
        {
            GenericResultSet<CoreAdminDataIndex> result = new GenericResultSet<CoreAdminDataIndex>();
            result.ResultSet = new CoreAdminDataIndex();
            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
            var fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
            try
            {
                IEnumerable<object> res = _CRUD.Set(type);
                result.ResultSet.Query = res.ToList();
                result.ResultSet.Navigations = _CRUD.GetNavigations(type).ToList();
                //result.ResultSet.Properties = _CRUD.GetProperties(type).ToList();             
                result.UserMessage = $"{fullName} executed successfully";
                result.InternalMessage = $"{fullName} executed successfully";
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.UserMessage = $"ERROR: {fullName} : {ex.Message}";
                result.InternalMessage = $"ERROR: {fullName} : {ex.Message}";
            }
            return result;
        }
    }
}
