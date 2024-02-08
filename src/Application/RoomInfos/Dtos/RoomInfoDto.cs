using Application.RoomTypes.Dtos;

namespace Application.RoomInfos.Dtos;
public class RoomInfoDto
{
    public Guid Id { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public string Description { get; set; } = string.Empty;
    public RoomTypeDto? RoomType { get; set; } = null!;
    public Guid? HotelId { get; set; }
}
