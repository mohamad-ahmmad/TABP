namespace Application.Rooms.Dtos;
public class RoomForCreationDto
{
    public string RoomNumber { get; set; } = string.Empty;
    public Guid RoomInfoId { get; set; }
    public int PricePerDay { get; set; }
}
