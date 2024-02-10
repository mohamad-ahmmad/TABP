using Application.Abstractions.Messaging;
using Application.Amenities.Dtos;

namespace Application.Amenities.Command.Create;
public record CreateAmenityCommand(AmenityForCreationDto AmenityForCreationDto) : ICommand<AmenityDto?>
{
}