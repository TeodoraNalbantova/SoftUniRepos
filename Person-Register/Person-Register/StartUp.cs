using Microsoft.EntityFrameworkCore;
using PersonRegister.Data.Models;
using PersonRegister.Data;
using PersonRegister.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Person_Register.Data.Models;

namespace PersonRegister
{
    public class StartUp
    {
        static async Task Main(string[] args)
        {
            var contextFactory = new PersonRegisterDbContextFactory();
            var dbContext = contextFactory.CreateDbContext(args);

            await dbContext.Database.MigrateAsync();


            var region = new Region()
            {
                Name = "Borovo",
                Persons = new List<Person> { }
            };
            dbContext.Regions.Add(region);
            dbContext.SaveChanges();
            var person = new Person()
            {
                FirstName = "Teodora",
                LastName = "Nalbantova",
                City = "Sofia",
                Age = 35,
                RegionId = region.Id,
                
            };

            dbContext.Persons.Add(person);
            await dbContext.SaveChangesAsync();

            //var person = dbContext.Persons.Where(p => p.FirstName == "Ivan");

            //var statement  = dbContext.Persons.OrderByDescending(p => p.LastName);

            //var person3 = dbContext.Persons.Any(p => p.FirstName == "Ivan");


            //var person4 = dbContext.Persons.Take(2).ToArray();

            //var person5 = dbContext.Persons.All(p => p.FirstName == "Ivan");



        }

    }
}
