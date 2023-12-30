using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Persistence.Repositories.Cities;
public class CitiesRepository : ICitiesRepository
{
    private readonly TABPDbContext _dbContext;

    public CitiesRepository(TABPDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<City> AddCityAsync(City city, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(city, cancellationToken);
        return city;
    }
}

