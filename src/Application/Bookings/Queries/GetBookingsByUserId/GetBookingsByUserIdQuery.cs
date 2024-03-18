using Application.Abstractions.Messaging;
using Application.Bookings.Dtos;
using Application.Dtos;

namespace Application.Bookings.Queries.GetBookingsByUserId;
public record GetBookingsByUserIdQuery(Guid UserId,
    string? SortOrder,
    string? SortCol,
    int Page,
    int PageSize) : IQuery<PagedList<BookingDto>>
{
}
