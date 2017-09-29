using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapRepo.Sample.Models;

namespace DapRepo.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter first name: ");
            var firstName = Console.ReadLine();
            Console.Write("Enter Last name: ");
            var lastName = Console.ReadLine();
            Console.Write("Enter age: ");
            var age = Convert.ToInt32(Console.ReadLine());

            var person = new Person()
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age
            };

            List<Person> persons = new List<Person>();

            using (var uow = new AppUnitOfWork(isTransactional: true))
            {
                uow.PersonRepository.Insert(person);
                uow.CommitTransaction();

                persons.AddRange(uow.PersonRepository.GetAll());
            }

            Console.WriteLine("\r\n**** Person List ****");
            
            foreach (var p in persons)
            {
                Console.WriteLine($"Id: {p.Id} \t FirstName: {p.FirstName} \t LastName: {p.LastName} \t Age: {p.Age} \t FullName: {p.FullName}");
            }

            Console.WriteLine("\r\n");
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }


}
