using Domain.Common;

namespace Domain.Entities;
public class RoomInfo : BaseSoftDeletableEntity
{
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid? RoomTypeId { get; set; }
    public RoomType? RoomType { get; set;} = null!;
    public Guid? HotelId { get; set; }
    public Hotel? Hotel { get; set; } = null!;
    public List<Room> Rooms { get; set; } = new List<Room>();
}
