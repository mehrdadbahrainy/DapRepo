using System.Configuration;
using System.Data;
using System.Transactions;
using DapRepo.DataAccess;
using DapRepo.Sample.Repositories;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace DapRepo.Sample
{
    internal class AppUnitOfWork : UnitOfWork
    {
        public AppUnitOfWork(bool isTransactional = false,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            int transactionTimeoutInSecond = 300,
            TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required) 
            :base(isTransactional, isolationLevel, transactionTimeoutInSecond, transactionScopeOption)
        {
        }

        protected override IDbConnection CreateConnection()
        {
            var connection = new System.Data.SqlClient.SqlConnection(connectionString: ConfigurationManager.ConnectionStrings["constr"].ConnectionString);
            return connection;
        }

        private PersonRepository _personRepository;
        public PersonRepository PersonRepository => _personRepository ?? (_personRepository = new PersonRepository(this.DbConnection));
    }
}
