using DAL.DataContext;
using DAL.Entities.Utils.ExtensionsMethods;
using DAL.Entities.Utils.Pagination;
using Microsoft.EntityFrameworkCore;
using DAL.CoreAdminExtensions;

namespace DAL.Functions.CRUD
{
    public class CRUD : ICRUD
    {
        private DatabaseContext _dbContext;
        public CRUD(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> Create<T>(T objectForDB) where T : class
        {
            try
            {
                await _dbContext.AddAsync<T>(objectForDB);
                await _dbContext.SaveChangesAsync();
                return objectForDB;
            }
            catch
            {
                throw;
            }
        }

        public async Task<T> Read<T>(object entityID) where T : class
        {
            try
            {
                T result = await _dbContext.FindAsync<T>(entityID);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<T>> ReadAll<T>() where T : class
        {
            try
            {
                List<T> result = await _dbContext.Set<T>().ToListAsync();
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<T> Update<T>(T entityToUpdate, object entityID) where T : class
        {
            try
            {
                var objectFound = await _dbContext.FindAsync<T>(entityID);
                if (objectFound != null)
                {
                    _dbContext.Entry(objectFound).CurrentValues.SetValues(entityToUpdate);
                    await _dbContext.SaveChangesAsync();
                }
                return objectFound;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete<T>(object entityID) where T : class
        {
            try
            {
                var objectFound = await _dbContext.FindAsync<T>(entityID);
                if (objectFound != null)
                {
                    _dbContext.Remove(objectFound);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;

            }
            catch
            {
                throw;
            }
        }

        public async Task<List<T>> ReadPaged<T>(int page, int pageSize) where T : class
        {
            try
            {
                PagedResult<T> result = await _dbContext.Set<T>().GetPaged(page, pageSize);
                return (List<T>)result.Results;
            }
            catch
            {
                throw;
            }
        }

        public IQueryable<object> Set(Type type) 
        {
            try
            {
                return (IQueryable<object>)(DbContext)_dbContext.Set(type);
                
            }
            catch
            {
                throw;
            }
        }
    }
}
