using Domain.Enums;

namespace Application.Abstractions;

public interface IUserContext
{
    Guid GetUserId();
    UserLevels GetUserLevel();
}

