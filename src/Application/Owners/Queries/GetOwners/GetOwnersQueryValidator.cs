using FluentValidation;

namespace Application.Owners.Queries.GetOwners;
public class GetOwnersQueryValidator : AbstractValidator<GetOwnersQuery>
{
    public GetOwnersQueryValidator()
    {
        RuleFor(o => o.PageSize)
            .Must(ps => ps >= 1 && ps <= 30)
            .WithMessage("Page size must be between 1 & 30");
    }
}
