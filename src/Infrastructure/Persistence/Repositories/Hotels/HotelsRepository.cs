using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Repositories.Hotels;
public class HotelsRepository : IHotelsRepository
{
    private readonly TABPDbContext _dbContext;
    private readonly ILogger<HotelsRepository> _logger;

    public HotelsRepository(TABPDbContext dbContext, ILogger<HotelsRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task AddHotelAsync(Hotel hotel, CancellationToken cancellationToken)
    {
        await _dbContext.Hotels.AddAsync(hotel, cancellationToken);
        _logger.LogInformation("Hotel with '{hotelId}' ID has been Tracked as '{state}'",
            hotel.Id, "EntityState.Added");
    }

    public async Task<Hotel?> GetHotelByIdAsync(Guid hotelId, CancellationToken cancellationToken)
    {
        return await _dbContext.Hotels.Include(h=> h.HotelType).FirstOrDefaultAsync(h => h.Id == hotelId, cancellationToken);
    }
}

