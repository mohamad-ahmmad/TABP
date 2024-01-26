using Domain.Shared;

namespace Domain.Errors;
public class HotelErrors
{
    public readonly static Error ForbidToCreateHotel = new Error("Create a hotel", "No permission");
    public readonly static Error ForbidToPatchHotel = new Error("Patch a hotel", "No permission");
    public readonly static Error ForbidToDeleteHotel = new Error("Delete a hotel", "No permission");
    public readonly static Error HotelNotFound = new Error("Hotel", "Not found");
}

