using Domain.Entities;

namespace Domain.Repositories;
public interface IHotelTypesRepository
{
    Task<HotelType> AddHotelTypeAsync(HotelType hotelType, CancellationToken cancellationToken);
    Task<HotelType?> GetHotelTypeByIdAsync(Guid hotelTypeId, CancellationToken cancellationToken);
}
