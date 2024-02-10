namespace Application.Amenities.Dtos;
public class AmenityDto
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public string Description { get; set; } = string.Empty;
}
