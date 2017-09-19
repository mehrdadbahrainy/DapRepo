using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Dapper;

namespace DapRepo.DataAccess
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IDbConnection DbConnection;

        protected GenericRepository(IDbConnection dbConnection)
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

        public void Insert(T entity)
        {
            var propertyValues = GetEntityProperties(entity);
            var keyInfo = GetEntityKeyInfo();
            var sql = $"INSERT INTO [{EntityName}] ({string.Join(", ", propertyValues.Keys)}) VALUES(@{string.Join(", @", propertyValues.Keys)}) SELECT CAST(scope_identity() AS {GetSqlDataType(keyInfo.PropertyType)})";
            var result = DbConnection.Query(sql, propertyValues, commandType: CommandType.Text).First() as IDictionary<string, object>;
            if (result != null)
            {
                var keyValue = result.Values.ToArray()[0];
                keyInfo.SetValue(entity, Convert.ChangeType(keyValue, keyInfo.PropertyType));
            }
        }

        public void Insert(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, object> GetEntityProperties(T entity)
        {
            var propertyValues = new Dictionary<string, object>();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                // Skip reference types except string
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    continue;

                // Skip methods without a public setter
                if (property.GetSetMethod() == null)
                    continue;

                // Skip methods specifically ignored
                if (property.IsDefined(typeof(IgnoreAttribute), false))
                    continue;

                if (property.IsDefined(typeof(KeyAttribute), false) && !property.IsDefined(typeof(NotDbGeneratedAttribute), false))
                    continue;

                var value = property?.GetValue(entity, null);
                propertyValues.Add(property.Name, value);

            }

            return propertyValues;
        }

        private PropertyInfo GetEntityKeyInfo()
        {
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if (property.IsDefined(typeof(KeyAttribute), false))
                {
                    return property;
                }
            }

            return null;
        }

        private string GetSqlDataType(Type type)
        {
            if (type == typeof(int))
            {
                return "INT";
            }
            else if (type == typeof(long))
            {
                return "BIGINT";
            }
            else if (type == typeof(byte))
            {
                return "TINYINT";
            }
            else if (type == typeof(short))
            {
                return "SMALLINT";
            }
            else if (type == typeof(Guid))
            {
                return "UNIQUEIDENTOFIER";
            }


            throw new Exception("Key type not supported");
        }
    }
}
