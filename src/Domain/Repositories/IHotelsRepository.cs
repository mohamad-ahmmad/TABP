using Domain.Entities;

namespace Domain.Repositories;
public interface IHotelsRepository
{
    Task AddHotelAsync(Hotel hotel, CancellationToken cancellationToken);
    Task<Hotel?> GetHotelByIdAsync(Guid hotelId, CancellationToken cancellationToken);
}

