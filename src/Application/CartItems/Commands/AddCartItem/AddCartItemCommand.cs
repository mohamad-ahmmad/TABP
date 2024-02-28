using Application.Abstractions.Messaging;
using Application.CartItems.Dtos;


namespace Application.CartItems.Commands.AddCartItem;
public record AddCartItemCommand(CartItemForCreationDto CartItemDto) : ICommand<CartItemDto>
{
}
