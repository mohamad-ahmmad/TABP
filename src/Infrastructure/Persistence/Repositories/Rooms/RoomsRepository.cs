using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infrastructure.Persistence.Repositories.Rooms;
public class RoomsRepository : IRoomsRepository
{
    private readonly TABPDbContext _dbContext;

    public RoomsRepository(TABPDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddRoomAsync(Room room, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(room, cancellationToken);
    }

    public async Task<Result<object?>> DeleteRoomByIdAsync(Guid roomId, CancellationToken cancellationToken)
    {
        var room = await _dbContext.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);
        if (room == null)
        {
            return Result<object?>.Failure(RoomErrors.NotFoundRoom, HttpStatusCode.NotFound);
        }

        room.IsDeleted = true;
        return Result<object?>.Success(HttpStatusCode.NoContent);
    }

    public async Task<IEnumerable<Room>> GetInvalidRoomsByBookingDateTimeIntervalAsync(IEnumerable<CartItem> cartItems,
        CancellationToken cancellationToken)
    {
        var invalidRooms = await _dbContext.Bookings
            .Where(
                b => cartItems.Any(
                    ci => 
                    ci.RoomId == b.RoomId &&
                    b.FromDate <= ci.FromDate &&
                    b.ToDate >= ci.ToDate
                    )
            )
            .Select(b => b.Room)
            .ToListAsync(cancellationToken);

        return invalidRooms!;
    }
}
