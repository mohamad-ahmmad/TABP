using Domain.Shared;

namespace Domain.Errors;
public static class OwnerErrors
{
    public static readonly Error ForbidToCreateOwner= new("Create a owner", "No permission");
    public static readonly Error ForbidToDeleteOwner= new("Delete a owner", "No permission");
    public static readonly Error ForbidToReadOwner= new("Read owners", "No permission");
    public static readonly Error ForbidToUpdateOwner= new("Update owner", "No permission");
    public static readonly Error OwnerNotFound= new("Owner", "Owner not found");
}
