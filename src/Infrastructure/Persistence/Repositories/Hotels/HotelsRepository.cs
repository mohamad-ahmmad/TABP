using Application.Abstractions;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Extensions;
using Infrastructure.Persistence.Repositories.Hotels.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories.Hotels;
public class HotelsRepository : IHotelsRepository
{
    private readonly TABPDbContext _dbContext;
    private readonly ILogger<HotelsRepository> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public HotelsRepository(TABPDbContext dbContext,
        ILogger<HotelsRepository> logger,
        IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
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

    
    public async Task<(IEnumerable<Hotel>, int)> GetHotelsAndTotalCount(int page,
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
        int? numberOfAdults,
        int? numberOfChildren,
        int? numberOfRooms,
        CancellationToken cancellationToken)
    {
        sortCol = sortCol?.ToLower();
        numberOfAdults ??= 2;
        numberOfChildren ??= 0;
        numberOfRooms ??= 1;

        IQueryable<Hotel> query = _dbContext.Hotels.Include(h => h.HotelType)
            .Where(h => h.IsDeleted == false);
        
        //TODO : SEARCH USING THE DISCOUNTED PRICE 
        //r.Discounts.Where(Perdicate).Select(d => d.DiscountPercentage).Take(1).FirstOrDefault()

        if (minPrice != null && maxPrice != null)
        {
            query = query.Where(h => h.RoomInfos.Any(
                ri => ri.Rooms.Any(r => r.PricePerDay >= minPrice
                && r.PricePerDay <= maxPrice)
                )
            );
        }
        else if (maxPrice != null)
        {
            query = query.Where(h => h.RoomInfos.Any(
                ri => ri.Rooms.Any(r => r.PricePerDay <= maxPrice)
                )
            );
        }
        else if (minPrice != null)
        {
            query = query.Where(h => h.RoomInfos.Any(
                ri => ri.Rooms.Any(r => r.PricePerDay >= minPrice)
                )
            );
        }


        if (hotelRating != null)
        {
            query = query.Where(h => h.StarRatingAcc / h.NumberOfPeopleRated >= hotelRating);
        }

        if (roomType != null)
        {
            query = query.Where(h => h.RoomInfos.Any(ri => ri.RoomType!.Name == roomType));
        }
        
        if (hotelType != null)
        {
            query = query.Where(h => h.HotelType!.Type == hotelType);
        }

        if (amenities != null)
        {
            amenities = amenities.ToLower();
            query = query.Where(h => h.Amenities.Any(a => amenities.Contains(a.Description)));
        }

        if (searchTerm != null)
        {
            query = query.Where(h => h.HotelName.Contains(searchTerm) ||
            h.StreetNme.Contains(searchTerm));
        }
        
        if (sortOrder?.ToLower() == "desc")
        {
            query = query.OrderByDescending(GetSortProperty(sortCol));
        }
        else
        {
            query = query.OrderBy(GetSortProperty(sortCol));
        }

        List<HotelAndDiscount> pagedHotels;
        
        if (sortCol == "discountpercentage")
            pagedHotels = await (from hotel in query
                                 join roomInfo in _dbContext.RoomInfos on hotel.Id equals roomInfo.HotelId
                                 join room in _dbContext.Rooms on roomInfo.Id equals room.RoomInfoId
                                 join discount in _dbContext.Discounts on room.Id equals discount.RoomId
                                 where discount.FromDate.CompareTo(_dateTimeProvider.GetUtcNow()) <= 0 &&
                                 discount.ToDate.CompareTo(_dateTimeProvider.GetUtcNow()) >= 0
                                 select new HotelAndDiscount
                                 {
                                     Hotel = hotel,
                                     Discount = new Discount
                                     {
                                         DiscountPercentage = discount.DiscountPercentage,
                                     }
                                 })
                .ToPagedListAsync(page, pageSize, cancellationToken);
        
        else
            pagedHotels = await query.Select(h => new HotelAndDiscount
            {
                Hotel = h
            }).ToPagedListAsync(page, pageSize, cancellationToken);


        var hotels = pagedHotels
            .Select(hd =>
            {
                Hotel hotel = hd.Hotel;
                RoomInfo ri = new();
                Room r = new();
                Discount discount = hd.Discount!;

                hotel.RoomInfos.Add(ri);
                ri.Rooms.Add(r);
                r.Discounts.Add(discount);
                return hotel;
            })
            .ToList();
        
        var numberOfHotels = await query.CountAsync(cancellationToken);
        
        return new(hotels, numberOfHotels);
    }
    
    private Expression<Func<Hotel, object>> GetSortProperty(string? sortCol)
    {
        
        if (sortCol == "discountpercentage")
        {
            return h => h.RoomInfos
                        .SelectMany(ri => ri.Rooms)
                        .SelectMany(r => r.Discounts)
                        .Where(d => d.FromDate.CompareTo(_dateTimeProvider.GetUtcNow()) <= 0 &&
                            d.ToDate.CompareTo(_dateTimeProvider.GetUtcNow()) >= 0)
                        .Select(d => d.DiscountPercentage)
                        .FirstOrDefault();
        }

        Expression<Func<Hotel, object>> keySelector = sortCol switch
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
        return await _dbContext.Hotels.Include(h => h.HotelType).FirstOrDefaultAsync(h => h.Id == hotelId && h.IsDeleted == false, cancellationToken);
    }
}

