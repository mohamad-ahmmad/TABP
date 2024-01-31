using Application.Abstractions.Messaging;
using MediatR;

namespace Application.RoomTypes.Commands.DeleteRoomTypeById;
public record DeleteRoomTypeByIdCommand(Guid RoomTypeId) : ICommand<Unit>
{
}
