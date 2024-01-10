using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Repositories.HotelTypes;
public class HotelTypesRepository : IHotelTypesRepository
{
    private readonly TABPDbContext _dbContext;
    private readonly ILogger<HotelTypesRepository> _logger;

    public HotelTypesRepository(TABPDbContext dbContext, ILogger<HotelTypesRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task<HotelType> AddHotelTypeAsync(HotelType hotelType, CancellationToken cancellationToken)
    {
        await _dbContext.HotelTypes.AddAsync(hotelType, cancellationToken);
        _logger.LogInformation("HotelType with '{hId}' being tracked as '{state}'", hotelType.Id, "EntityState.Added");
        return hotelType;
    }
}

