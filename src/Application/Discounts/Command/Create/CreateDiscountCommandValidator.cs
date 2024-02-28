using Application.Abstractions;
using FluentValidation;

namespace Application.Discounts.Command.Create;
public class CreateDiscountCommandValidator : AbstractValidator<CreateDiscountCommand>
{
    public CreateDiscountCommandValidator(IDateTimeProvider dateProvider)
    {
        var utcNow = dateProvider.GetUtcNow();

        RuleFor(c => c.DiscountDto.RoomId)
            .NotEmpty();

        RuleFor(c => c.DiscountDto.DiscountPercentage)
            .NotEmpty();

        RuleFor(c => c.DiscountDto.FromDate)
            .NotEmpty()
            .Must(df => df.CompareTo(utcNow.Date) >= 0 )
            .WithMessage("Invalid FromDate field");

        RuleFor(c => c.DiscountDto.ToDate)
            .NotEmpty()
            .Must(dt => dt.CompareTo(utcNow.Date) >= 0)
            .WithMessage("Invalid ToDate field");

        RuleFor(c => c.DiscountDto)
            .Must(d => d.ToDate.CompareTo(d.FromDate) > 0)
            .WithMessage("Invalid date fields");
    }
}
