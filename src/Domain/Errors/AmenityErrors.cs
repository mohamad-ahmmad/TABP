using Domain.Shared;

namespace Domain.Errors;
public static class AmenityErrors
{
    public readonly static Error ForbidToCreateAmenity = new("Create an amenity", "No permission");

}

