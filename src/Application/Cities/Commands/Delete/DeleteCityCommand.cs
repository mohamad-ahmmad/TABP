using Application.Abstractions.Messaging;
using MediatR;

namespace Application.Cities.Commands.Delete;
public record DeleteCityCommand(Guid CityId) : ICommand<Unit>
{
}

