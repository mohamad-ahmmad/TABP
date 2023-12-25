using Domain.Shared;

namespace Domain.Errors;

public static class UserErrors
{
    public static Error UsernameAlreadyUsed = new("User.Username", "The username is already taken.");
    public static Error EmailAlreadyUsed = new("User.Email", "The email is already taken.");
    public static Error UnAuthorized = new("User.UserLevel", "UnAuthorized");
    public static Error InvalidCredentials = new("Credentials", "Invalid username or password.");
}

