namespace Application.RoomInfos.Dtos;
public class RoomInfoForCreateDto
{
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid? RoomTypeId { get; set; }
}
