using Application.Abstractions.Messaging;
using Application.RoomTypes.Dtos;

namespace Application.RoomTypes.Commands.CreateRoomType;
public record CreateRoomTypeCommand(RoomTypeForCreateDto RoomTypeForCreateDto) : ICommand<RoomTypeDto>
{
}
