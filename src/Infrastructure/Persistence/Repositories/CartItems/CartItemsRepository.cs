using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Persistence.Repositories.CartItems;
internal class CartItemsRepository : ICartItemsRepository
{
    private readonly TABPDbContext _dbContext;

    public CartItemsRepository(TABPDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddCartItemAsync(CartItem cartItem, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(cartItem, cancellationToken);
    }
}
