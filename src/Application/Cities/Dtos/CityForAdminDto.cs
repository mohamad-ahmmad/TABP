namespace Application.Cities.Dtos;

#nullable disable
public class CityForAdminDto
{
    public string CityName { get; set; } 
    public string CountryName { get; set; } 
    public string ThumbnailUrl { get; set; }
    public DateTimeOffset Created { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public Guid LastModifiedBy { get; set; }
}

