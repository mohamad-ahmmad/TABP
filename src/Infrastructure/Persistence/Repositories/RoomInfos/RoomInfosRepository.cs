using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infrastructure.Persistence.Repositories.RoomInfos;
public class RoomInfosRepository : IRoomInfosRepository
{
    private readonly TABPDbContext _dbContext;

    public RoomInfosRepository(TABPDbContext dbContext)
    {
        _dbContext = dbContext;    
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
            .Where(r => r.IsDeleted == false && r.HotelId == hotelId);

        if(roomType != null)
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

        var roomInfos = await roomInfosQuery.ToListAsync(cancellationToken);

        return roomInfos;
    }
}

