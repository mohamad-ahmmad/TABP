using Application.Abstractions.Messaging;
using Application.Discounts.Dtos;

namespace Application.Discounts.Command.Create;
public record CreateDiscountCommand(DiscountDto DiscountDto) : ICommand<DiscountDto>
{
}

