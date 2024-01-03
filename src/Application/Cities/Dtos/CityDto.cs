namespace Application.Cities.Dtos;

#nullable disable
public class CityDto
{
    public Guid Id { get; set; }
    public string CityName { get; set; }
    public string CountryName { get; set; }
    public string ThumbnailUrl { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public string PostOfficePostalCode { get; set; } = string.Empty;
    public DateTime? Created { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public bool IsAdmin { get; set; } = true;
}

