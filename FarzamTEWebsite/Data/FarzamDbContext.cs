using FarzamTEWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace FarzamTEWebsite.Data
{
    public class FarzamDbContext : DbContext
    {
        public FarzamDbContext(DbContextOptions<FarzamDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<HappyCall> HappyCalls { get; set; }
    }
}
