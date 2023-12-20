using FluentValidation;

namespace Application.Users.Queries.Login;

public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
{
    public LoginUserQueryValidator()
    {
        RuleFor(q => q.CredentialsDto.Username)
            .NotEmpty();
        RuleFor(q => q.CredentialsDto.Password)
        .NotEmpty();
    }
}

