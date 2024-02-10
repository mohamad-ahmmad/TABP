using Domain.Common;

namespace Domain.Entities;
public class Amenity : BaseSoftDeletableEntity
{
    public string Description { get; set; } = string.Empty;
    public Guid? HotelId { get; set; }
    public Hotel? Hotel { get; set; } = null!;
}
