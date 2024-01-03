using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
    
    public async Task<bool> DoesCityExistsAsync(string cityName, string countryName, CancellationToken cancellationToken)
    {
        cityName = cityName.ToLower();
        countryName = countryName.ToLower();
        return await _dbContext.Cities.Where(c => c.CityName.Equals(cityName) || c.CountryName.Equals(countryName)).AnyAsync(cancellationToken);
    }

    public async Task<Tuple<IEnumerable<City>, int>> GetCitiesAsync(string? searchTerm,
        string? sortCol,
        string? sortOrder,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        IQueryable<City> citiesQuery = _dbContext.Cities;

        if(searchTerm != null)
        {
            citiesQuery.Where(c => 
            c.CityName.Contains(searchTerm) || 
            c.CountryName.Contains(searchTerm));
        }

        if(sortOrder?.ToLower() == "desc")
        {
            citiesQuery.OrderByDescending(GetSortProperty(sortCol));
        }
        else
        {
            citiesQuery.OrderBy(GetSortProperty(sortCol));
        }

        int count = await citiesQuery.CountAsync(cancellationToken);
        var cities = await citiesQuery.ToPagedListAsync(page, pageSize, cancellationToken);


        return new(cities, count);
    }

    private Expression<Func<City, object>> GetSortProperty(string? sortCol)
    {
        Expression<Func<City, object>> keySelector = sortCol?.ToLower() switch
        {
            "countryname" => c => c.CountryName,
            "latitude" => c => c.Latitude,
            "longitude" => c => c.Longitude,
            "cityname" => c => c.CityName,
            _ => c => c.CityName,
        };

        return keySelector;
    }

    public async Task<City?> GetCityByIdAsync(Guid Id, CancellationToken cancellationToken)
    {
        return await _dbContext.Cities.Where(c=> c.Id == Id).FirstOrDefaultAsync();
    }
}

