using Application.Abstractions.Messaging;
using MediatR;

namespace Application.Owners.Commands.Delete;
public record DeleteOwnerCommand(Guid OwnerId) : ICommand<Unit>
{
}
