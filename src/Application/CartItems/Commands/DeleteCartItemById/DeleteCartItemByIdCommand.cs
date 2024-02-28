using Application.Abstractions.Messaging;
using Domain.Shared;

namespace Application.CartItems.Commands.DeleteCartItemById;
public record DeleteCartItemByIdCommand(Guid CartItemId, Guid UserId) : ICommand<Empty>
{   
}