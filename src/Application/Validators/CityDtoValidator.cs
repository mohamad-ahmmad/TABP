using Application.Cities.Dtos;
using FluentValidation;

namespace Application.Validators;
public class CityDtoValidator : AbstractValidator<CityDto>
{
    public CityDtoValidator()
    {
        RuleFor(c => c.CityName)
    .NotEmpty();
        RuleFor(c => c.CountryName)
            .NotEmpty();
        RuleFor(c => c.Latitude)
            .NotEmpty();
        RuleFor(c => c.PostOfficePostalCode)
            .NotEmpty();
        RuleFor(c => c.Longitude)
            .NotEmpty();
    }
}

