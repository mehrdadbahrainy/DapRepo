using System.Collections.Generic;

namespace DapRepo.DataAccess
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> GetAll();
        void Insert(T entity);
        void Insert(IEnumerable<T> entities);
        void Update(T entity);
        void Update(IEnumerable<T> entities);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
        void DeleteAll();
    }
}