using Application.Abstractions.Messaging;
using Application.Owners.DTOs;

namespace Application.Owners.Commands.Create;
public record CreateOwnerCommand(OwnerForCreateDto OwnerForCreateDto) : ICommand<OwnerDto>
{
}