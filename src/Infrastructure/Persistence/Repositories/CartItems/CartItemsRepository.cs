using Application.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infrastructure.Persistence.Repositories.CartItems;
public class CartItemsRepository : ICartItemsRepository
{
    private readonly TABPDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CartItemsRepository(TABPDbContext dbContext, 
        IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }
    public async Task<Result<Empty>> AddCartItemAsync(CartItem cartItem, CancellationToken cancellationToken)
    {
        var roomAndDiscount = await (from r in _dbContext.Rooms
                                     join d in _dbContext.Discounts.Where(d => d.FromDate.CompareTo(_dateTimeProvider.GetUtcNow()) <= 0
                                     && d.ToDate.CompareTo(_dateTimeProvider.GetUtcNow()) >= 0)
                                     on r.Id equals d.RoomId into rd
                                     from d in rd.DefaultIfEmpty()
                                     where r.Id == cartItem.RoomId
                                     select new
                                     {
                                         Room = r,
                                         Discount = d
                                     }).FirstOrDefaultAsync(cancellationToken);
        if (roomAndDiscount == null)
        {
            return Result<Empty>.Failure(RoomErrors.NotFoundRoom, HttpStatusCode.NotFound);
        }
        cartItem.Room = roomAndDiscount.Room;
        
        if(roomAndDiscount.Discount != null)
        {
            cartItem.Room.Discounts.Add(roomAndDiscount.Discount);
        }
        await _dbContext.AddAsync(cartItem, cancellationToken);
        
    

        return Result<Empty>.Success(HttpStatusCode.Created)!;
    }
}
