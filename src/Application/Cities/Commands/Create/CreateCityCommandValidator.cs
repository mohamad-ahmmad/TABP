using Application.Abstractions;
using FluentValidation;

namespace Application.Cities.Commands.Create;

public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
{
    public CreateCityCommandValidator(IImageExtensionValidator imageExtensionValidator)
    {
        RuleFor(c => c.CityDto.CityName)
            .NotEmpty();
        RuleFor(c => c.CityDto.CountryName)
            .NotEmpty();
        RuleFor(c => c.CityDto.Image)
            .NotNull()
            .Must(img => imageExtensionValidator.Validate(Path.GetExtension(img.FileName)))
            .WithMessage("Invalid file extension.");
    }
}

