using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Cities.Dtos;
using MediatR;

namespace Application.Cities.Commands.Update;
public record UpdateCityCommand(IPatchRequest<CityDto> PatchRequest ,Guid CityId) : ICommand<Unit>
{
}
