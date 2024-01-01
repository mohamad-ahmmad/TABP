using Domain.Entities;

namespace Domain.Repositories;

public interface ICitiesRepository
{
    Task<City> AddCityAsync(City city, CancellationToken cancellationToken);
    Task<bool> DoesCityExistsAsync(string cityName, string countryName, CancellationToken cancellationToken);
    Task<City?> GetCityByIdAsync(Guid Id, CancellationToken cancellationToken);
}

