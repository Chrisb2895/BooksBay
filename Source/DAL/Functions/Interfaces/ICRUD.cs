namespace DAL
{
    public interface ICRUD
    {
        Task<T> Create<T>(T entity) where T : class;

        Task<T> Read<T>(object entityID) where T : class;

        Task<List<T>> ReadAll<T>() where T : class;

        Task<List<T>> ReadPaged<T>(int page, int pageSize) where T : class;

        Task<T> Update<T>(T entityToUpdate, object entityID) where T : class;

        Task<bool> Delete<T>(object entityID) where T : class;

        IQueryable<object> Set(Type type);
    }
}