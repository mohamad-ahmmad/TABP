using Domain.Common;

namespace Domain.Entities;

public class City : BaseSoftDeletableAuditableEntity
{
    public string CityName { get; set; }= string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string PostOfficePostalCode { get; set; } = string.Empty;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

