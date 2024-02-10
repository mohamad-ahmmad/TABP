using Domain.Common;

namespace Domain.Entities;
public class Hotel : BaseSoftDeletableAuditableEntity
{
    public string HotelName { get; set; } = string.Empty;
    public int NumberOfRooms { get; set; } = 0;
    public Guid? CityId { get; set; }
    public City? City { get; set; } = null!;
    public long StarRatingAcc { get; set; }
    public string Description { get; set; } = string.Empty;
    public long NumberOfPeopleRated { get; set; }
    public Guid? HotelTypeId { get; set; }
    public HotelType? HotelType { get; set; } = null!;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string StreetNme { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public Guid? OwnerId { get; set;}
    public Owner? Owner { get; set; } = null!;
    public List<Amenity> Amenities { get; set; } = new List<Amenity>();
}
