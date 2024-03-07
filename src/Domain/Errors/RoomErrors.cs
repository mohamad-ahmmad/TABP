using Domain.Shared;

namespace Domain.Errors;
public class RoomErrors
{
    public readonly static Error ForbidToCreateRoom = new("Create a room", "No permission");
    public readonly static Error ForbidToDeleteRoom = new("Delete a room", "No permission");
    public readonly static Error NotFoundRoom = new("Room", "Not found");
    public static Error RoomWithIdIsAlreadyBookedError(Guid roomId)
    {
        return new Error("Book a room", $"room with '{roomId}' is booked");
    }
}

