using Application.CartItems.Dtos;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Net;

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

}
