using Application.Abstractions.Messaging;
using Application.RoomInfos.Dtos;

namespace Application.RoomInfos.Commands.Create;
public record CreateRoomInfoCommand(RoomInfoForCreateDto RoomInfoForCreateDto, Guid HotelId) : ICommand<RoomInfoDto?>
{
}
