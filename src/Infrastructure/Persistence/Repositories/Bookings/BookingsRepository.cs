using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories.Bookings;
public class BookingsRepository : IBookingsRepository
{
    private readonly TABPDbContext _dbContext;

    public BookingsRepository(TABPDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddBookingAsync(Booking booking, CancellationToken cancellationToken)
    {
        await _dbContext.Bookings
            .AddAsync(booking);
    }

    public async Task<Tuple<IEnumerable<Booking>, int>> GetBookingsAsync(int page,
        int pageSize,
        string? sortCol,
        string? sortOrder,
        CancellationToken cancellationToken)
    {
        sortOrder = sortOrder?.ToLower();


        IQueryable<Booking> bookingsQuery = _dbContext.Bookings;

        if (sortOrder == "desc")
        {
            bookingsQuery = bookingsQuery.OrderByDescending(GetSortProperty(sortCol));
        }
        else
        {
            bookingsQuery = bookingsQuery.OrderBy(GetSortProperty(sortCol));
        }

        var bookings = await bookingsQuery.ToPagedListAsync(page, pageSize, cancellationToken);
        var count = await bookingsQuery.CountAsync(cancellationToken);

        return new Tuple<IEnumerable<Booking>, int>(bookings, count);
    }

    private Expression<Func<Booking, object>> GetSortProperty(string? sortCol)
    {
        return sortCol switch
        {
            "todate" => b => b.ToDate,
            "fromdate" => b => b.FromDate,
            _ => b => b.Id
        };
    }

}
