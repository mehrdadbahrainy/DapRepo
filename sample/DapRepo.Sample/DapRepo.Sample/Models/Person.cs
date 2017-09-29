using DapRepo.DataAccess;

namespace DapRepo.Sample.Models
{
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
}
