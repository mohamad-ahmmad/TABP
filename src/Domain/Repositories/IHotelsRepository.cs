using Domain.Entities;

namespace Domain.Repositories;
public interface IHotelsRepository
{
    Task AddHotelAsync(Hotel hotel, CancellationToken cancellationToken);
}

