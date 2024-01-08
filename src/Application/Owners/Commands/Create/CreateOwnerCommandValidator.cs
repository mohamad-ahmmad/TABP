using FluentValidation;

namespace Application.Owners.Commands.Create;
public class CreateOwnerCommandValidator : AbstractValidator<CreateOwnerCommand>
{
    public CreateOwnerCommandValidator()
    {
        RuleFor(e => e.OwnerForCreateDto.Email)
        .Matches("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$")
        .WithMessage("Invalid email");

        RuleFor(e => e.OwnerForCreateDto.FirstName)
            .NotEmpty();

        RuleFor(e => e.OwnerForCreateDto.LastName)
            .NotEmpty();

        RuleFor(e => e.OwnerForCreateDto.PhoneNumber)
            .NotEmpty();

    }
}

