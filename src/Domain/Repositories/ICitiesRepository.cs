using Domain.Entities;

namespace Domain.Repositories;

public interface ICitiesRepository
{
    Task<City> AddCityAsync(City city, CancellationToken cancellationToken);
}

