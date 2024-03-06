using Domain.Entities;
using Domain.Shared;

namespace Domain.Repositories;
public interface IRoomsRepository
{
    Task AddRoomAsync(Room room, CancellationToken cancellationToken);
    Task<Result<object?>> DeleteRoomByIdAsync(Guid roomId, CancellationToken cancellationToken);
    Task<IEnumerable<Room>> GetInvalidRoomsByBookingDateTimeIntervalAsync(IEnumerable<CartItem> cartItems,
        CancellationToken cancellationToken);
}
