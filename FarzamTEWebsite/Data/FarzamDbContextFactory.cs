using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FarzamTEWebsite.Data
{
    public class FarzamDbContextFactory : IDesignTimeDbContextFactory<FarzamDbContext>
    {
        public FarzamDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<FarzamDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new FarzamDbContext(optionsBuilder.Options);
        }
    }
}
