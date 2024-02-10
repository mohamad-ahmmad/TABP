namespace Application.Amenities.Dtos;
public class AmenityForCreationDto
{
    public string Description { get; set; } = string.Empty;
    public Guid? HotelId { get; set; }
}
