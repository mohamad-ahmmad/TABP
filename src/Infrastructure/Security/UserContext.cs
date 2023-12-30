using Application.Abstractions;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security;
public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;   
    }
    
    public Guid GetUserId()
    {
        string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "user_id")?.Value!;

        return Guid.Parse(userId);
    }

    public UserLevels GetUserLevel()
    {
        string userLevel = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "user_role")?.Value!;
        return Enum.Parse<UserLevels>(userLevel);
    }
}

