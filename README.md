# DapRepo
Dapper Generic Repository And Unit Of Work

## Instalation
**Last Release:** Version 1.1.0 (September 27, 2017)

    PM> Install-Package Hurdad.DataAccess.DapRepo
    
## Usage

*Create models:*

    public class Person
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        [Ignore]
        public string FullName => $"{FirstName} {LastName}";
    }
    
    
*Create repository:*

    class PersonRepository : GenericRepository<Person>
    {
        public PersonRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }

*Create unit-of-work class:*

    class AppUnitOfWork : UnitOfWork
    {
        public UnitOfWork(bool isTransactional = false,
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
    

*Then you can use instance of Unit-of-work class in your project:*

    using (var uow = new UnitOfWork(isTransactional: true))
    {
        uow.PersonRepository.Insert(person);
        uow.CommitTransaction();
    
        persons.AddRange(uow.PersonRepository.GetAll());
    }


#### You can find full example project in [sample](https://github.com/mehrdadbahrainy/DapRepo/tree/master/sample) directory
