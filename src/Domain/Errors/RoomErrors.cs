using Domain.Shared;

namespace Domain.Errors;
public class RoomErrors
{
    public readonly static Error ForbidToCreateRoom = new Error("Create a room", "No permission");
}

