using Domain.Entities;

namespace Domain.Repositories;
public interface IHotelsRepository
{
    Task AddHotelAsync(Hotel hotel, CancellationToken cancellationToken);
    Task<Hotel?> GetHotelByIdAsync(Guid hotelId, CancellationToken cancellationToken);
    Task<bool> DeleteHotelByIdAsync(Guid hotelId, CancellationToken cancellationToken);
    Task<(IEnumerable<Hotel>, int)> GetHotelsAndTotalCount(int page,
        int pageSize,
        int? minPrice,
        int? maxPrice,
        double? hotelRating,
        string? amenities,
        string? hotelType,
        string? roomType,
        string? searchTerm,
        string? sortCol,
        string? sortOrder,
        int? numberOfAdults,
        int? numberOfChildren,
        int? numberOfRooms,
        CancellationToken cancellationToken);
}

