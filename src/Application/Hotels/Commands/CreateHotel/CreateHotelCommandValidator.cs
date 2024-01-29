using FluentValidation;

namespace Application.Hotels.Commands.CreateHotel;
public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator()
    {
        //Add validation
        RuleFor(h => h.HotelForCreateDto.Longitude)
            .NotEmpty();
        RuleFor(h => h.HotelForCreateDto.ThumbnailImage).NotEmpty();
        RuleFor(h => h.HotelForCreateDto.OwnerId).NotEmpty();
        RuleFor(h => h.HotelForCreateDto.Description).NotEmpty();
        RuleFor(h => h.HotelForCreateDto.HotelName).NotEmpty();
        RuleFor(h => h.HotelForCreateDto.StreetNme).NotEmpty();
        RuleFor(h => h.HotelForCreateDto.PostalCode).NotEmpty();
        RuleFor(h => h.HotelForCreateDto.CityId).NotEmpty();
    }
}

