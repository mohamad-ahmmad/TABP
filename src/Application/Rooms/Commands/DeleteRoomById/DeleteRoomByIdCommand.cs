using Application.Abstractions.Messaging;
using MediatR;

namespace Application.Rooms.Commands.DeleteRoomById;
public record DeleteRoomByIdCommand(Guid RoomId) : ICommand<Unit>
{
}
