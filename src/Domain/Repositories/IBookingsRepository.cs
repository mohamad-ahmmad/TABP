using Domain.Entities;
using Domain.Shared;

namespace Domain.Repositories;
public interface IBookingsRepository
{
    Task AddBookingAsync(Booking booking, CancellationToken cancellationToken);
}

