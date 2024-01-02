using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Application.Cities.Dtos;
public class CityForCreateDto
{
    public string CityName { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string PostOfficePostalCode { get; set; } = string.Empty;
    [JsonIgnore]
    public IFormFile Image { get; set; } = null!;
}

