using Domain.Shared;

namespace Domain.Errors;
public static class OwnerErrors
{
    public static readonly Error ForbidToCreateOwner= new("Create a owner", "No permission");
}
