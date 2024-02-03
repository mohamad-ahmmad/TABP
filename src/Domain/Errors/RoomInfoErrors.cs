using Domain.Shared;

namespace Domain.Errors;
public static class RoomInfoErrors
{
    public readonly static Error NotFoundRoomInfo = new("Room Info", "Not found");
}

