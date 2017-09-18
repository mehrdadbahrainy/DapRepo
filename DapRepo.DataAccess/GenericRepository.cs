using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DapRepo.DataAccess
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IDbConnection DbConnection;

        public GenericRepository(IDbConnection dbConnection)
        {
            this.DbConnection = dbConnection;
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        int IGenericRepository<T>.Insert(T obj)
        {
            throw new NotImplementedException();
        }

        public int Insert(IEnumerable<T> list)
        {
            throw new NotImplementedException();
        }

        public bool Update(T obj)
        {
            throw new NotImplementedException();
        }

        public bool Update(IEnumerable<T> list)
        {
            throw new NotImplementedException();
        }

        public bool Delete(T obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(IEnumerable<T> list)
        {
            throw new NotImplementedException();
        }

        public bool DeleteAll()
        {
            throw new NotImplementedException();
        }

        public void Insert(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
