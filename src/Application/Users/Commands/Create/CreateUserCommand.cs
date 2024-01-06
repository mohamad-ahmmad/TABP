using Application.Abstractions.Messaging;
using Application.Users.DTOs;
using Domain.Shared;
using MediatR;

namespace Application.Users.Commands.Create;

public record CreateUserCommand(UserForCreateDTO UserForCreateDto) : ICommand<UserDto> { }