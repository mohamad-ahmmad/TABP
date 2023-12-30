using Application.Cities.Dtos;
using Domain.Shared;
using MediatR;

namespace Application.Cities.Commands.Create;

public record CreateCityCommand(CityForCreateDto CityDto) : IRequest<Result<CityForAdminDto>>
{
}

