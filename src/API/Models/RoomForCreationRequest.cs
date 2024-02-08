namespace API.Models;
public class RoomForCreationRequest
{
    public string RoomNumber { get; set; } = string.Empty;
    public int PricePerDay { get; set; }
}
