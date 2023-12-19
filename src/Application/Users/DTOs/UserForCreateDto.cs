using Domain.Enums;

namespace Application.Users.DTOs
{
    public class UserForCreateDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public UserLevels UserLevel { get; set; } = UserLevels.User;
    }
}
