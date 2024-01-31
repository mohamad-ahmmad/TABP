using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infrastructure.Persistence.Repositories.RoomTypes;
public class RoomTypesRepository : IRoomTypesRepository
{
    private readonly TABPDbContext _dbContext;

    public RoomTypesRepository(TABPDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddRoomTypeAsync(RoomType roomType, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(roomType, cancellationToken);
    }

    public async Task<Result<object?>> DeleteRoomTypeByIdAsync(Guid roomTypeId, CancellationToken cancellationToken)
    {
        var roomType = await _dbContext.RoomTypes.FirstOrDefaultAsync(rt => rt.Id == roomTypeId);
        if(roomType == null)
        {
            return Result<object?>.Failure(RoomTypeErrors.RoomTypeNotFound, HttpStatusCode.NotFound);
        }
        roomType.IsDeleted = true;
        return Result<object?>.Success(HttpStatusCode.NoContent);
    }

    public async Task<IEnumerable<RoomType>> GetAllRoomTypesAsync(CancellationToken cancellationToken)
    {
        var roomTypes = await _dbContext.RoomTypes.ToListAsync(cancellationToken);
        return roomTypes;
    }
}
