using Application.Abstractions.Messaging;
using MediatR;

namespace Application.Hotels.Commands.DeleteHotelById;
public record DeleteHotelByIdCommand(Guid HotelId) : ICommand<Unit>
{
}

