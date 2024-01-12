using Domain.Shared;

namespace Domain.Errors;
public static class HotelTypeErrors
{
    public static readonly Error ForbidToCreateHotelType = new("Create a hotel type", "No permission");
    public static readonly Error HotelTypeNotFound = new("Hotel type", "Not found");
}

