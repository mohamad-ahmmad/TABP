using Microsoft.AspNetCore.Http;

namespace Application.Hotels.Dtos;
public class HotelForCreateDto
{
    public string HotelName { get; set; } = string.Empty;
    public int NumberOfRooms { get; set; } = 0;
    public Guid? CityId { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid? HotelTypeId { get; set; }
    public string StreetNme { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public Guid? OwnerId { get; set; }
    public IFormFile ThumbnailImage { get; set; } = null!;
}

