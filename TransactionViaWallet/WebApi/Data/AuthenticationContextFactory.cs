using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebAPI.Models
{
    public class AuthenticationContextFactory : IDesignTimeDbContextFactory<AuthenticationContext>
    {
        public AuthenticationContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AuthenticationContext>();
            var connectionString = configuration.GetConnectionString("IdentityConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new AuthenticationContext(optionsBuilder.Options);
        }
    }
}
    