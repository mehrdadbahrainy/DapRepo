using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace DapRepo.DataAccess
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IDbConnection DbConnection;

        public GenericRepository(IDbConnection dbConnection)
        {
            this.DbConnection = dbConnection;
        }

        public string EntityName => typeof(T).Name;

        public IEnumerable<T> GetAll()
        {
            var query = $"SELECT * FROM {EntityName}";
            var results = DbConnection.Query<T>(query);
            return results;
        }

        int IGenericRepository<T>.Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public int Insert(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public bool Update(T entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public bool Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(IEnumerable<T> entities)
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
