using Application.Abstractions.Messaging;
using Application.Rooms.Dtos;

namespace Application.Rooms.Commands.Create;
public record CreateRoomCommand(RoomForCreationDto RoomForCreationDto) : ICommand<RoomDto?>
{
}
