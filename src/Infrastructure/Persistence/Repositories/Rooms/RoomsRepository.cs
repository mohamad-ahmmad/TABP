using Domain.Entities;
using Domain.Repositories;

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
}
