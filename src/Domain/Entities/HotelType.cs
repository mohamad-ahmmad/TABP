using Domain.Common;

namespace Domain.Entities;
public class HotelType : BaseSoftDeletableAuditableEntity
{
    public string Type { get; set; } = string.Empty;
    public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
}