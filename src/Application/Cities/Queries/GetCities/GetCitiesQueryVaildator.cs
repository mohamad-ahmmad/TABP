using FluentValidation;

namespace Application.Cities.Queries.GetCities;
public class GetCitiesQueryVaildator : AbstractValidator<GetCitiesQuery>
{
    public GetCitiesQueryVaildator()
    {
        RuleFor(q => q.pageSize)
            .Must(pageSize => pageSize <= 50 && pageSize >= 1)
            .WithMessage("Maximum page size is 50");
        RuleFor(q => q.SearchTerm)
            .MaximumLength(50)
            .WithMessage("Maximum SearchTerm is 50");
        RuleFor(q => q.sortCol)
            .MaximumLength(20)
            .WithMessage("Maximum sortCol length is 20");
    }
}

