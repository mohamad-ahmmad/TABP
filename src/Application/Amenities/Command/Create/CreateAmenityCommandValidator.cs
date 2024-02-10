using FluentValidation;

namespace Application.Amenities.Command.Create;
public class CreateAmenityCommandValidator : AbstractValidator<CreateAmenityCommand>
{
    public CreateAmenityCommandValidator()
    {
        RuleFor(a => a.AmenityForCreationDto.Description).NotEmpty();
    }
}
