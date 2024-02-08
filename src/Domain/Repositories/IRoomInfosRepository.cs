using Domain.Entities;
using Domain.Shared;

namespace Domain.Repositories;
public interface IRoomInfosRepository
{
    Task AddRoomInfoAsync(RoomInfo roomInfo, Guid hotelId, CancellationToken cancellationToken);
    Task<Result<object?>> DeleteRoomInfoByIdAsync (Guid roomInfoId, CancellationToken cancellationToken);
    Task<IEnumerable<RoomInfo>> GetAllRoomInfosAsync(Guid hotelId, CancellationToken cancellationToken);
}

