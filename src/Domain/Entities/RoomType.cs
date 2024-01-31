using Domain.Common;

namespace Domain.Entities;
public class RoomType : BaseSoftDeletableEntity
{
    public string Name { get; set; } = string.Empty;
}
