using Application.Abstractions.Messaging;
using Application.Cities.Dtos;
using Domain.Shared;
using MediatR;
using System.Windows.Input;

namespace Application.Cities.Commands.Create;

public record CreateCityCommand(CityForCreateDto CityDto) : ICommand<CityForAdminDto>
{
}

