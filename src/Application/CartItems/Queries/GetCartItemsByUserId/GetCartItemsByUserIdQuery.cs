using Application.Abstractions.Messaging;
using Application.CartItems.Dtos;

namespace Application.CartItems.Queries.GetCartItemsByUserId;
public record GetCartItemsByUserIdQuery(Guid UserId) : IQuery<IEnumerable<CartItemDto>>
{
}
