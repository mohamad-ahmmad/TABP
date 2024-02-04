using Application.Abstractions.Messaging;
using MediatR;

namespace Application.RoomInfos.Commands.DeleteRoomInfoById;
public record DeleteRoomInfoByIdCommand(Guid RoomInfoId) : ICommand<Unit>
{
}
