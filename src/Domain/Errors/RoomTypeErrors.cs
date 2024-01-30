using Domain.Shared;

namespace Domain.Errors;
public class RoomTypeErrors
{
    public readonly static Error RoomTypeNotFound = new("RoomType", "Room type not found");
    public readonly static Error ForbidToCreateRoomType = new("Create a room type", "No permission");
}
