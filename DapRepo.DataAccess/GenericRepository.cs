using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
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

        protected string EntityName => typeof(T).Name;

        public virtual IEnumerable<T> GetAll()
        {
            var query = $"SELECT * FROM {EntityName}";
            var results = DbConnection.Query<T>(query);
            return results;
        }

        public virtual IEnumerable<T> GetAllPaged(int limit, int offset)
        {
            var query = $"SELECT * FROM {EntityName} ORDER BY Id DESC OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY";
            var results = DbConnection.Query<T>(query, new { Limit = limit, Offset = offset });
            return results;
        }

        public async Task<IEnumerable<T>> GetAllPagedAsync(int limit, int offset)
        {
            var query = $"SELECT * FROM {EntityName} ORDER BY Id DESC OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY";
            var results = await DbConnection.QueryAsync<T>(query, new { Limit = limit, Offset = offset });
            return results;
        }

        public virtual void Insert(T entity)
        {
            var propertyValues = GetEntityProperties(entity);
            var keyInfo = GetEntityKeyInfo();
            var sql = $"INSERT INTO [{EntityName}] ({string.Join(", ", propertyValues.Keys)}) VALUES(@{string.Join(", @", propertyValues.Keys)}) SELECT CAST(scope_identity() AS {GetSqlDataType(keyInfo.PropertyType)})";
            var result = DbConnection.Query(sql, propertyValues, commandType: CommandType.Text).First() as IDictionary<string, object>;
            if (result != null && !keyInfo.IsDefined(typeof(NotDbGeneratedAttribute), false))
            {
                var keyValue = result.Values.ToArray()[0];
                keyInfo.SetValue(entity, Convert.ChangeType(keyValue, keyInfo.PropertyType));
            }
        }

        public virtual void Insert(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
        }

        public virtual void Update(T entity)
        {
            var propertyValues = GetEntityProperties(entity);
            var keyInfo = GetEntityKeyInfo();
            var keyPairs = $"{keyInfo.Name} = @{keyInfo.Name}";
            var pairs = propertyValues.Where(key => key.Key != keyInfo.Name).Select(key => $"{key.Key}=@{key.Key}");
            var updateParameters = string.Join(", ", pairs);
            var sql = $"UPDATE [{EntityName}] SET {updateParameters} WHERE {keyPairs}";
            DbConnection.Execute(sql, entity, commandType: CommandType.Text);
        }

        public virtual void Update(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        public virtual void Delete(T entity)
        {
            var keyInfo = GetEntityKeyInfo();
            var keyPairs = $"{keyInfo.Name} = @{keyInfo.Name}";
            var sql = $"DELETE FROM [{EntityName}] WHERE {keyPairs}";
            DbConnection.Execute(sql, entity, commandType: CommandType.Text);
        }

        public virtual void Delete(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public virtual void DeleteAll()
        {
            var sql = $"DELETE FROM [{EntityName}]";
            DbConnection.Execute(sql, commandType: CommandType.Text);
        }

        private Dictionary<string, object> GetEntityProperties(T entity)
        {
            var propertyValues = new Dictionary<string, object>();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                // Skip reference types except string
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string) && property.PropertyType != typeof(byte[]))
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
                return "UNIQUEIDENTIFIER";
            }


            throw new Exception("Key type not supported");
        }
    }
}
