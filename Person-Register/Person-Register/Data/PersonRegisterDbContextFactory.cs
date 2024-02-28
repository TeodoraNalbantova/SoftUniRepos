using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;


namespace PersonRegister.Data
{
    public class PersonRegisterDbContextFactory : IDesignTimeDbContextFactory<PersonRegisterDbContext>
    {
        public PersonRegisterDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PersonRegisterDbContext>();
            var connectionString = DbConfiguration.ConnectionString;
            builder.UseSqlServer(connectionString);

            return new PersonRegisterDbContext(builder.Options);
        }
    }
}
