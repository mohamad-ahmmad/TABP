using FluentValidation;

namespace Application.Users.Commands.Create;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {

        RuleFor(e => e.UserForCreateDto.Email)
            .Matches("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$")
            .WithMessage("Invalid email");
        RuleFor(e => e.UserForCreateDto.Username)
            .NotEmpty()
            .MaximumLength(30)
            .MinimumLength(5);
        RuleFor(e => e.UserForCreateDto.Password)
            .Matches("^(?=.*[A-Z].*[A-Z])(?=.*[!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8}$")
            //TODO: return more information.
            .WithMessage("Weak password");
        RuleFor(e => e.UserForCreateDto.Firstname)
            .NotEmpty()
            .MaximumLength(30)
            .MinimumLength(2);
        RuleFor(e => e.UserForCreateDto.Lastname)
            .NotEmpty()
            .MaximumLength(30)
            .MinimumLength(2);
    }
}

