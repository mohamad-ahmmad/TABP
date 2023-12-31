using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

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
        city.CityName = city.CityName.Trim().ToLower();
        city.CountryName = city.CountryName.Trim().ToLower();

        await _dbContext.AddAsync(city, cancellationToken);
        return city;
    }
    
    public async Task<bool> DoesCityExists(string cityName, string countryName, CancellationToken cancellationToken)
    {
        cityName = cityName.ToLower();
        countryName = countryName.ToLower();
        return await _dbContext.Cities.Where(c => c.CityName.Equals(cityName) || c.CountryName.Equals(countryName)).AnyAsync(cancellationToken);
    }
}

