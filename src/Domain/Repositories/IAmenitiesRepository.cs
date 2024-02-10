using Domain.Entities;

namespace Domain.Repositories;
public interface IAmenitiesRepository
{
    Task AddAmenityAsync(Amenity amenity, CancellationToken cancellationToken);
}

