using Application.Owners.DTOs;
using FluentValidation;

namespace Application.Owners.Validators;
public class OnwerDtoValidator : AbstractValidator<OwnerDto>
{
    public OnwerDtoValidator()
    {
        RuleFor(e => e.Email)
        .Matches("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$")
        .WithMessage("Invalid email");

        RuleFor(e => e.FirstName)
            .NotEmpty();

        RuleFor(e => e.LastName)
            .NotEmpty();

        RuleFor(e => e.PhoneNumber)
            .NotEmpty();
    }
}
