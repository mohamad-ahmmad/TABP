using FluentValidation;

namespace Application.HotelTypes.Commands.Create;
public class CreateHotelTypeCommandValidator : AbstractValidator<CreateHotelTypeCommand>
{
    public CreateHotelTypeCommandValidator()
    {
        RuleFor(ht => ht.HotelTypeDto.Type)
            .NotEmpty();
    }
}
