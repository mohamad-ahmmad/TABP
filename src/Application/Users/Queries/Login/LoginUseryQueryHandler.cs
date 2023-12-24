using Application.Abstractions;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.Login;

public class LoginUseryQueryHandler : IRequestHandler<LoginUserQuery, Result<string>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IHasher _hasher;
    private readonly ILogger<LoginUseryQueryHandler> _logger;

    public LoginUseryQueryHandler
        (
            IJwtProvider jwtProvider,
            IUsersRepository usersRepository,
            IHasher hasher,
            ILogger<LoginUseryQueryHandler> logger
        )
    {
        _usersRepository = usersRepository;
        _jwtProvider = jwtProvider;
        _hasher = hasher;
        _logger = logger;
    }
    public async Task<Result<string>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var credentials = request.CredentialsDto;
        var user = await _usersRepository.GetUserByCredentials(credentials.Username,
                                                           _hasher.Hash(credentials.Password),
                                                            cancellationToken);
        if (user == null)
        {
            _logger.LogInformation($"Invalid credentials.");
            return UserErrors.InvalidCredentials;
        }

        var token = _jwtProvider.GenerateToken(user);
        _logger.LogInformation($"User with {credentials.Username} username has generated '{token}' JWT Token.");
        return token;
    }
}

