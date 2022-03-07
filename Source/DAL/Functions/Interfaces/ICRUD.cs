using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL
{
    public interface ICRUD
    {
        Task<T> Create<T>(T entity) where T : class;

        Task<T> Read<T>(Int64 entityID) where T : class;

        Task<List<T>> ReadAll<T>() where T : class;

        Task<T> Update<T>(T entityToUpdate,Int64 entityID) where T : class;

        Task<bool> Delete<T>(Int64 entityID) where T : class;
    }
}