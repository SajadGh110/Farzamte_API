using AuthenticationPlugin;
using FarzamTEWebsite.Models;

namespace FarzamTEWebsite.Data.Initialization
{
    public class AdminSeeder
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FarzamDbContext>();

            if (!dbContext.Users.Any())
            {
                var admin = new User
                {
                    UserName = "admin",
                    Password = SecurePasswordHasherHelper.Hash("Admin123"),
                    Role = "Owner",
                    FirstName = "System",
                    LastName = "Admin",
                    Broker = "mobin",
                    Email = "admin@system.local"
                };

                dbContext.Users.Add(admin);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
