using Domain.Entities;

namespace Domain.Repositories;
public interface IRoomsRepository
{
    Task AddRoomAsync(Room room, CancellationToken cancellationToken);
}
