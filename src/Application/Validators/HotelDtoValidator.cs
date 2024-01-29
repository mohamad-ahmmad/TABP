using Application.Hotels.Dtos;
using FluentValidation;

namespace Application.Validators;
public class HotelDtoValidator : AbstractValidator<HotelDto>
{
    public HotelDtoValidator()
    {
        //Todo:Add more validation
        RuleFor(h => h.Longitude)
    .NotEmpty();
        RuleFor(h => h.OwnerId).NotEmpty();
        RuleFor(h => h.Description).NotEmpty();
        RuleFor(h => h.HotelName).NotEmpty();
        RuleFor(h => h.StreetNme).NotEmpty();
        RuleFor(h => h.PostalCode).NotEmpty();
        RuleFor(h => h.CityId).NotEmpty();
    }
}

