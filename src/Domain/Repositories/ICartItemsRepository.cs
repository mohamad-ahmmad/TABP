using Domain.Entities;
using Domain.Shared;

namespace Domain.Repositories;
public interface ICartItemsRepository
{
    Task<Result<Empty>> AddCartItemAsync(CartItem cartItem, CancellationToken cancellationToken);
    Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
