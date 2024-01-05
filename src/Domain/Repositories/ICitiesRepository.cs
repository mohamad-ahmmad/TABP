using Domain.Entities;

namespace Domain.Repositories;

public interface ICitiesRepository
{
    Task<City> AddCityAsync(City city, CancellationToken cancellationToken);
    Task<bool> DoesCityExistsAsync(string cityName, string countryName, CancellationToken cancellationToken);
    Task<City?> GetCityByIdAsync(Guid Id, CancellationToken cancellationToken);
    Task<Tuple<IEnumerable<City>, int>> GetCitiesAsync(string? searchTerm,
        string? sortCol,
        string? sortOrder,
        int page,
        int pageSize,
        CancellationToken cancellationToken);
    void DeleteCityById(Guid Id);
    Task<bool> DoesCityExistsByIdAsync(Guid cityId, CancellationToken cancellationToken);
}

