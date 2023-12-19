using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class TABPDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public TABPDbContext(DbContextOptions<TABPDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            
        }
    }
}
