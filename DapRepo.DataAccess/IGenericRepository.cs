using System.Collections.Generic;

namespace DapRepo.DataAccess
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> GetAll();
        int Insert(T entity);
        int Insert(IEnumerable<T> entities);
        bool Update(T entity);
        bool Update(IEnumerable<T> entities);
        bool Delete(T entity);
        bool Delete(IEnumerable<T> entities);
        bool DeleteAll();
    }
}