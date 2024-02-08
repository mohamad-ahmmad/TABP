using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Infrastructure.Persistence;

public class TABPDbContext : DbContext
{

    public DbSet<User> Users { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<HotelType> HotelTypes { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<RoomInfo> RoomInfos { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<Room> Rooms { get; set; }

    public TABPDbContext(DbContextOptions<TABPDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder mb)
    {

        mb.Entity<Hotel>()
            .HasOne(h => h.City)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);
            
        
        mb.Entity<Hotel>()
           .HasOne(h => h.HotelType)
           .WithMany(ht => ht.Hotels)
           .HasForeignKey(h => h.HotelTypeId)
           .OnDelete(DeleteBehavior.SetNull);

        mb.Entity<Hotel>()
            .HasOne(h => h.Owner)
            .WithMany(o => o.Hotels)
            .HasForeignKey(h => h.OwnerId)
            .OnDelete(DeleteBehavior.SetNull);
        mb.Entity<RoomInfo>()
            .HasOne(ri => ri.RoomType)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);


        SeedingUsers(mb);
        SeedingCities(mb);
    }

    private void SeedingCities(ModelBuilder mb)
    {
        mb.Entity<City>().HasData
            (
                new City
                {
                    Id = Guid.NewGuid(),
                    CityName = "Japan",
                    CountryName = "Tokyo",
                    Longitude = 33.1245,
                    Latitude = 1.12314,
                    PostOfficePostalCode="Z32Z",
                    ThumbnailUrl = "1.jpg"
                },
                new City
                {
                    Id = Guid.NewGuid(),
                    CityName = "Moscow",
                    CountryName = "Russia",
                    Longitude = 13.1245,
                    Latitude = 1.12314,
                    PostOfficePostalCode = "X32Z",
                    ThumbnailUrl = "2.jpg"
                }
            );
    }

    private void SeedingUsers(ModelBuilder mb)
    {
        mb.Entity<User>().HasData(
                new User
                {
                    Id = new Guid("8c667e48-0b4f-49a8-a964-9c44698bc860"),
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
                    Id =  new Guid("c3fea012-2148-41b4-9b76-b6a30293bf5d"),
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
