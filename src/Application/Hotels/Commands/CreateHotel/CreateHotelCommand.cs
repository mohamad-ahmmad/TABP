using Application.Abstractions.Messaging;
using Application.Hotels.Dtos;

namespace Application.Hotels.Commands.CreateHotel;
public record CreateHotelCommand(HotelForCreateDto HotelForCreateDto) : ICommand<HotelDto>
{
}
