using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapRepo.DataAccess
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllPaged(int limit, int offset);
        Task<IEnumerable<T>> GetAllPagedAsync(int limit, int offset);
        void Insert(T entity);
        void Insert(IEnumerable<T> entities);
        void Update(T entity);
        void Update(IEnumerable<T> entities);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
        void DeleteAll();
    }
}