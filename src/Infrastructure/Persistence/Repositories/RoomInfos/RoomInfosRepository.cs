using Application.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace Infrastructure.Persistence.Repositories.RoomInfos;
public class RoomInfosRepository : IRoomInfosRepository
{
    private readonly TABPDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RoomInfosRepository(TABPDbContext dbContext, 
        IDateTimeProvider dateProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateProvider;
    }

    public async Task AddRoomInfoAsync(RoomInfo roomInfo, Guid hotelId, CancellationToken cancellationToken)
    {
        roomInfo.HotelId = hotelId;
        await _dbContext.AddAsync(roomInfo, cancellationToken);
        roomInfo.RoomType = await _dbContext.RoomTypes.FirstAsync(rt => rt.Id == roomInfo.RoomTypeId && roomInfo.IsDeleted == false, cancellationToken);
    }

    public async Task<Result<object?>> DeleteRoomInfoByIdAsync(Guid roomInfoId, CancellationToken cancellationToken)
    {
        var roomInfo = await _dbContext.RoomInfos
            .FirstOrDefaultAsync(ri => ri.Id == roomInfoId && ri.IsDeleted == false, cancellationToken);
        if(roomInfo == null)
        {
            return Result<object?>.Failure(RoomInfoErrors.NotFoundRoomInfo, HttpStatusCode.NotFound);
        }
        roomInfo.IsDeleted = true;
        return Result<object?>.Success(HttpStatusCode.NoContent);
    }
    
    public async Task<IEnumerable<RoomInfo>> GetAllRoomInfosAsync(Guid hotelId,
        string? roomType,
        int? minPrice,
        int? maxPrice,
        CancellationToken cancellationToken)
    {
        var roomInfosQuery = _dbContext.RoomInfos
            .Include(ri => ri.RoomType)
            .Include(ri => ri.Rooms)
            .Where(r => r.IsDeleted == false && r.HotelId == hotelId);

        if (roomType != null)
        {
            roomInfosQuery = roomInfosQuery.Where(ri => ri.RoomType!.Name == roomType);
        }
        if (minPrice != null && maxPrice != null)
        {
            roomInfosQuery = roomInfosQuery.Where(ri => ri.Rooms.Any(r => r.PricePerDay >= minPrice && r.PricePerDay <= maxPrice))
            .Include(ri => ri.Rooms.Where(r => r.PricePerDay >= minPrice && r.PricePerDay <= maxPrice));
                                ;
        }
      
        else if (minPrice != null)
        {
            roomInfosQuery = roomInfosQuery.Where(ri => ri.Rooms.Any(r => r.PricePerDay >= minPrice))
           .Include(ri => ri.Rooms.Where(r => r.PricePerDay >= minPrice));
           
        }
        else if (maxPrice != null)
        {
            roomInfosQuery = roomInfosQuery.Where(ri => ri.Rooms.Any(r => r.PricePerDay <= maxPrice))
            .Include(ri => ri.Rooms.Where(r => r.PricePerDay <= maxPrice));
        }


        var roomInfos = await roomInfosQuery
            .Select(ri => new RoomInfo
            {
                Id = ri.Id,
                RoomTypeId = ri.RoomTypeId,
                AdultsCapacity = ri.AdultsCapacity,
                ChildrenCapacity = ri.ChildrenCapacity,
                Description = ri.Description,
                HotelId = ri.HotelId,
                IsDeleted = ri.IsDeleted,
                Rooms = ri.Rooms.Select(r => new Room
                {
                    Id = r.Id,
                    PricePerDay = r.PricePerDay,
                    IsDeleted = r.IsDeleted,
                    RoomNumber = r.RoomNumber,
                    RoomInfoId = r.RoomInfoId,
                    RoomInfo = r.RoomInfo,
                    Created = r.Created,
                    CreatedBy = r.CreatedBy,
                    LastModified = r.LastModified,
                    LastModifiedBy = r.LastModifiedBy,
                    Discounts = r.Discounts.Where(d =>
                                              d.FromDate.CompareTo(_dateTimeProvider.GetUtcNow()) <= 0 &&
                                              d.ToDate.CompareTo(_dateTimeProvider.GetUtcNow()) >= 0)
                    .Take(1)
                    .ToList(),
                }).ToList(),
                RoomType = ri.RoomType
            })
            .ToListAsync(cancellationToken);

        return roomInfos;
    }
}

