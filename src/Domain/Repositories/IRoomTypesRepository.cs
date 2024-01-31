using Domain.Entities;
using Domain.Shared;

namespace Domain.Repositories;
public interface IRoomTypesRepository
{
    public Task AddRoomTypeAsync(RoomType roomType, CancellationToken cancellationToken);
    public Task<Result<object?>> DeleteRoomTypeByIdAsync(Guid roomTypeId, CancellationToken cancellationToken);
    public Task<IEnumerable<RoomType>> GetAllRoomTypesAsync(CancellationToken cancellationToken);
}

