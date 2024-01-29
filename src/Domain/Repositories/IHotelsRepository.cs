using Domain.Entities;

namespace Domain.Repositories;
public interface IHotelsRepository
{
    Task AddHotelAsync(Hotel hotel, CancellationToken cancellationToken);
    Task<Hotel?> GetHotelByIdAsync(Guid hotelId, CancellationToken cancellationToken);
    Task<bool> DeleteHotelByIdAsync(Guid hotelId, CancellationToken cancellationToken);
    Task<(IEnumerable<Hotel>, int)> GetCitiesAndTotalCount(int page,
        int pageSize,
        string? searchTerm,
        string? sortCol,
        string? sortOrder,
        CancellationToken cancellationToken);
}

