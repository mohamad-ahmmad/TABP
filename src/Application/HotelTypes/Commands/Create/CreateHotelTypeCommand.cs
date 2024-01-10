using Application.Abstractions.Messaging;
using Application.HotelTypes.Dtos;


namespace Application.HotelTypes.Commands.Create;
public record CreateHotelTypeCommand(HotelTypeDto HotelTypeDto) : ICommand<HotelTypeDto>
{
}

