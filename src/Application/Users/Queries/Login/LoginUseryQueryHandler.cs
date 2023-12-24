using Application.Abstractions;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using MediatR;

namespace Application.Users.Queries.Login;

public class LoginUseryQueryHandler : IRequestHandler<LoginUserQuery, Result<string>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IHasher _hasher;

    public LoginUseryQueryHandler
        (
            IJwtProvider jwtProvider,
            IUsersRepository usersRepository,
            IHasher hasher 
        )
    {
        _usersRepository = usersRepository;
        _jwtProvider = jwtProvider;
        _hasher = hasher;
    }
    public async Task<Result<string>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var credentials = request.CredentialsDto;
        var user = await _usersRepository.GetUserByCredentials(credentials.Username,
                                                           _hasher.Hash(credentials.Password),
                                                            cancellationToken);
        if (user == null)
        {
            return UserErrors.InvalidCredentials;
        }

        var token = _jwtProvider.GenerateToken(user);
        return token;
    }
}

