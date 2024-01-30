using Domain.Shared;

namespace Domain.Errors;
public class RoomTypeErrors
{
    public static Error RoomTypeNotFound = new Error("RoomType", "Room type not found.");
}
