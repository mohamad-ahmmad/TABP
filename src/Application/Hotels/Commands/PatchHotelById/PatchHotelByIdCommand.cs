using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Hotels.Dtos;
using MediatR;

namespace Application.Hotels.Commands.PatchHotelById;
public record PatchHotelByIdCommand(Guid HotelId, IPatchRequest<HotelDto> PatchRequest) : ICommand<Unit>
{
}