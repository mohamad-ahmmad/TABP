using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories.Hotels;
public class HotelsRepository : IHotelsRepository
{
    private readonly TABPDbContext _dbContext;
    private readonly ILogger<HotelsRepository> _logger;

    public HotelsRepository(TABPDbContext dbContext, ILogger<HotelsRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task AddHotelAsync(Hotel hotel, CancellationToken cancellationToken)
    {
        await _dbContext.Hotels.AddAsync(hotel, cancellationToken);
        _logger.LogInformation("Hotel with '{hotelId}' ID has been Tracked as '{state}'",
            hotel.Id, "EntityState.Added");
    }

    public async Task<bool> DeleteHotelByIdAsync(Guid hotelId, CancellationToken cancellationToken)
    {
        var hotel = await _dbContext.Hotels.FirstOrDefaultAsync(h => h.Id == hotelId && h.IsDeleted == false);
        if (hotel == null)
        {
            return false;
        }

        hotel.IsDeleted = true;
        return true;
    }

    public async Task<(IEnumerable<Hotel>, int)> GetCitiesAndTotalCount(int page,
        int pageSize,
        int? minPrice,
        int? maxPrice,
        double? hotelRating,
        string? amenities,
        string? hotelType,
        string? roomType,
        string? searchTerm,
        string? sortCol,
        string? sortOrder,
        CancellationToken cancellationToken)
    {

        IQueryable<Hotel> query = _dbContext.Hotels.Include(h => h.HotelType)
            .Where(h => h.IsDeleted == false);
        
        if(minPrice != null)
        {
            query = query.Where(h => h.Rooms.Any(r => r.PricePerDay >= minPrice));
        }

        if(maxPrice != null)
        {
            query = query.Where(h => h.Rooms.Any(r => r.PricePerDay <= maxPrice));
        }

        if(hotelRating != null)
        {
            query = query.Where(h => h.StarRatingAcc/h.NumberOfPeopleRated >=  hotelRating);
        }

        if(amenities != null)
        {
            amenities = amenities.ToLower();
            query = query.Where(h => h.Amenities.Any(a => amenities.Contains(a.Description)));
        }

        if(searchTerm != null)
        {
            query = query.Where(h => h.HotelName.Contains(searchTerm) ||
            h.StreetNme.Contains(searchTerm));
        }
        
        if(sortOrder?.ToLower() == "desc")
        {
            query = query.OrderByDescending(GetSortProperty(sortCol));
        }
        else
        {
            query = query.OrderBy(GetSortProperty(sortCol));
        }


        var pagedHotels = await query.ToPagedListAsync(page, pageSize, cancellationToken);
        var numberOfHotels = await query.CountAsync();

        return new(pagedHotels, numberOfHotels);
    }

    private Expression<Func<Hotel, object>> GetSortProperty(string? sortCol)
    {
        Expression<Func<Hotel, object>> keySelector = sortCol?.ToLower() switch
        {
            "hotelname" => h => h.HotelName,
            "numberofrooms" => h => h.NumberOfRooms,
            "streetname" => h => h.StreetNme,
            "longitude" => h => h.Longitude,
            "latitude" => h => h.Latitude,
            _ => h => h.HotelName
        };

        return keySelector;
    }

    public async Task<Hotel?> GetHotelByIdAsync(Guid hotelId, CancellationToken cancellationToken)
    {
        return await _dbContext.Hotels.Include(h=> h.HotelType).FirstOrDefaultAsync(h => h.Id == hotelId && h.IsDeleted == false , cancellationToken);
    }
}

