using Application.Users.DTOs;
using Domain.Shared;
using MediatR;

namespace Application.Users.Queries.Login;

public record LoginUserQuery(LoginCredentialsDto CredentialsDto) : IRequest<Result<string>>
{
}
