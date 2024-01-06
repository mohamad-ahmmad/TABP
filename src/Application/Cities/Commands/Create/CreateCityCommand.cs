using Application.Abstractions.Messaging;
using Application.Cities.Dtos;

namespace Application.Cities.Commands.Create;

public record CreateCityCommand(CityForCreateDto CityDto) : ICommand<CityDto>
{
}

