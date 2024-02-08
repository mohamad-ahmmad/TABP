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
    
    public async Task<IEnumerable<RoomInfo>> GetAllRoomInfosAsync(Guid hotelId, CancellationToken cancellationToken)
    {
        var roomInfos = await _dbContext.RoomInfos
            .Include(ri => ri.RoomType)
            .Where(r => r.IsDeleted == false && r.HotelId == hotelId)
            .ToListAsync(cancellationToken);

        return roomInfos;
    }
}

