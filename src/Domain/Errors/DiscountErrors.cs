using Domain.Shared;

namespace Domain.Errors;
public static class DiscountErrors
{
    public readonly static Error ForbidToCreateDiscount = new("Create a discount", "No permission");
}

