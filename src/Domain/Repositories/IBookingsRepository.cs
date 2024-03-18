using Domain.Entities;
using Domain.Shared;

namespace Domain.Repositories;
public interface IBookingsRepository
{
    Task AddBookingAsync(Booking booking, CancellationToken cancellationToken);
    Task<Tuple<IEnumerable<Booking>, int>> GetBookingsAsync(int page,
        int pageSize,
        string? sortCol,
        string? sortOrder,
        CancellationToken cancellationToken);
}

