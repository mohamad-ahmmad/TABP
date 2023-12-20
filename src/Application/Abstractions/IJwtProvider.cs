using Domain.Common;

namespace Application.Abstractions;

public interface IJwtProvider<T> where T : BaseEntity
{
    string GenerateToken(T obj);
}

