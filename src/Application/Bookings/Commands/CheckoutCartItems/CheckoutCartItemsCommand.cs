using Application.Abstractions.Messaging;
using Domain.Shared;

namespace Application.Bookings.Commands.CheckoutCartItems;
public record CheckoutCartItemsCommand(string CardDetailsToken,
    string IdempotencyKey,
    Guid UserId) : ICommand<Empty>
{
}
