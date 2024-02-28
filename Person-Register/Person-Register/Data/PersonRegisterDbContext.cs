using Microsoft.EntityFrameworkCore;
using Person_Register.Data.Models;
using PersonRegister.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonRegister.Data
{
    public class PersonRegisterDbContext : DbContext 
    {
       
        public DbSet<Person> Persons { get; set; }
        public DbSet<Region> Regions { get; set; }

    
      public  PersonRegisterDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DbConfiguration.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>();
            modelBuilder.Entity<Region>();
        }

    }
}
