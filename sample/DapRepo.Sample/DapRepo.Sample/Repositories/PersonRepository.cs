using System.Data;
using DapRepo.DataAccess;
using DapRepo.Sample.Models;

namespace DapRepo.Sample.Repositories
{
    internal class PersonRepository : GenericRepository<Person>
    {
        public PersonRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}
