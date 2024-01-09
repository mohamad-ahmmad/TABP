using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Owners.DTOs;
using MediatR;

namespace Application.Owners.Commands.Update;
public record UpdateOwnerCommand(Guid OwnerId, IPatchRequest<OwnerDto> Patch ) : ICommand<Unit>
{
}

