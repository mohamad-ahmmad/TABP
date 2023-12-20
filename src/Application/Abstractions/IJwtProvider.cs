using Domain.Entities;

namespace Application.Abstractions;

public interface IJwtProvider
{
    string GenerateToken(User obj);
}

