using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Persistence.Repositories.Amenities;
public class AmenitiesRepository : IAmenitiesRepository
{
    private readonly TABPDbContext _dbContext;

    public AmenitiesRepository(TABPDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddAmenityAsync(Amenity amenity, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(amenity, cancellationToken); 
    }
}

