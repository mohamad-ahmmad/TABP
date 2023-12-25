using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class TABPDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public TABPDbContext(DbContextOptions<TABPDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            SeedingUsers(mb);
        }

        private void SeedingUsers(ModelBuilder mb)
        {
            mb.Entity<User>().HasData(
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "mohah",
                        Firstname = "Mohammad",
                        Lastname = "Ahmad",
                        Email = "mail@gmail.com",
                        UserLevel = UserLevels.User,
                        Password = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8",
                        IsDeleted = false,
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "ml7m",
                        Firstname = "Melheem",
                        Lastname = "Met'b",
                        Email = "ml7m@gmail.com",
                        UserLevel = UserLevels.User,
                        Password = "5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8",
                        IsDeleted = false,
                    }
                );
        }
    }
}
