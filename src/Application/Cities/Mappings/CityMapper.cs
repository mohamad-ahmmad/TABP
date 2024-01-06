using Application.Cities.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Cities.Mappings;
public static class CityMapper
{
    public static CityDto MapCityToCityDto(City city, IMapper mapper, bool isAdmin)
    {
        var cityDto = mapper.Map<CityDto>(city);
        if (!isAdmin)
        {
            HideAdminProperities(cityDto);
        }
        return cityDto;
    }
    public static IEnumerable<CityDto> MapCitiesToCitiesDto(IEnumerable<City> cities, IMapper mapper, bool isAdmin)
    {
        var citiesDto = mapper.Map<IEnumerable<CityDto>>(cities);
        
        if (!isAdmin)
        {
            foreach (var item in citiesDto)
            {
                HideAdminProperities(item);
            }

        }
        return citiesDto;
    }

    private static CityDto HideAdminProperities(CityDto cityDto)
    {
        cityDto.Created = null;
        cityDto.CreatedBy = null;
        cityDto.LastModified = null;
        cityDto.LastModifiedBy = null;
        cityDto.IsAdmin = false;

        return cityDto;
    }

}

