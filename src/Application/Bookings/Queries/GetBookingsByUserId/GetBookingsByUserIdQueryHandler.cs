using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Bookings.Dtos;
using Application.Dtos;
using AutoMapper;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using System.Net;

namespace Application.Bookings.Queries.GetBookingsByUserId;
public class GetBookingsByUserIdQueryHandler : IQueryHandler<GetBookingsByUserIdQuery, PagedList<BookingDto>>
{
    private readonly IUserContext _userContext;
    private readonly IBookingsRepository _bookingsRepo;
    private readonly IMapper _mapper;

    public GetBookingsByUserIdQueryHandler(IUserContext userContext,
        IBookingsRepository bookingsRepo,
        IMapper mapper
        )
    {
        _userContext = userContext;
        _bookingsRepo = bookingsRepo;
        _mapper = mapper;

    }
    public async Task<Result<PagedList<BookingDto>>> Handle(GetBookingsByUserIdQuery request, CancellationToken cancellationToken)
    {
        if (_userContext.GetUserId() != request.UserId)
        {
            return Result<PagedList<BookingDto>>.Failure(BookingErrors.ForbidToGetBookings, HttpStatusCode.Forbidden);
        }
        var bookingsAndTotalCount = await _bookingsRepo.GetBookingsAsync(request.Page,
            request.PageSize,
            request.SortCol,
            request.SortOrder,
            cancellationToken);

        var(bookings, count) = bookingsAndTotalCount;

        var bookingsDto = _mapper.Map<IEnumerable<BookingDto>>(bookings);
        var pagedList = new PagedList<BookingDto>(bookingsDto, request.Page, request.PageSize, count);
        return Result<PagedList<BookingDto>>.Success(pagedList)!;
    }
}
