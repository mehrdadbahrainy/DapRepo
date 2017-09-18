using System.Collections.Generic;

namespace DapRepo.DataAccess
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> GetAll();
        int Insert(T obj);
        int Insert(IEnumerable<T> list);
        bool Update(T obj);
        bool Update(IEnumerable<T> list);
        bool Delete(T obj);
        bool Delete(IEnumerable<T> list);
        bool DeleteAll();
    }
}