using Application.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.CompilerServices;

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

    public async Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(Guid userId,
        CancellationToken cancellationToken)
    {
        var cartItemsAndRoomAndDiscount = await (from ci in _dbContext.CartItems.Where(ci => ci.UserId == userId)
                                   join r in _dbContext.Rooms on ci.RoomId equals r.Id
                                   join d in _dbContext.Discounts.Where(d => d.FromDate.CompareTo(_dateTimeProvider.GetUtcNow()) <= 0
                                   && d.ToDate.CompareTo(_dateTimeProvider.GetUtcNow()) >= 0)
                                   on r.Id equals d.RoomId into rd
                                   from d in rd.DefaultIfEmpty()
                                   select new
                                   {
                                       CartItem = ci,
                                       Room = r,
                                       Discount = d
                                   }).ToListAsync(cancellationToken);

        var cartItems = cartItemsAndRoomAndDiscount
                           .Select(crd =>
                           {
                               var c = crd.CartItem;
                               c.Room = crd.Room;
                               if(crd.Discount != null)
                               {
                                   c.Room.Discounts.Add(crd.Discount);
                               }
                               
                               return c;
                           })
                           .ToList();

        return cartItems;
    }
}
