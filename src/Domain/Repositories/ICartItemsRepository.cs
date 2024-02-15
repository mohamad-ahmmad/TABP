using Domain.Entities;

namespace Domain.Repositories;
public interface ICartItemsRepository
{
    Task AddCartItemAsync(CartItem cartItem, CancellationToken cancellationToken);
}
