using Application.Hotels.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Hotels.Mappings;
public static class HotelMappingUtilities
{
    public static HotelDto MapHotelToHotelDto(Hotel hotel, UserLevels userLevel, IMapper mapper)
    {
        var hotelDto = mapper.Map<HotelDto>(hotel);
        hotelDto.IsAdmin = userLevel == UserLevels.Admin;
        if (!hotelDto.IsAdmin)
        {
            HideAdminProperities(hotelDto);
        }
        return hotelDto;
    }
    public static IEnumerable<HotelDto> MapHotelsToHotelsDto(IEnumerable<Hotel> hotels, UserLevels userLevel, IMapper mapper)
    {
        var hotelsDto = mapper.Map<IEnumerable<HotelDto>>(hotels);
        var isAdmin = userLevel == UserLevels.Admin;

        foreach (var h in hotelsDto)
        {
            if (!isAdmin)
            {
                HideAdminProperities(h);
            }
            h.IsAdmin = isAdmin;
        }

        return hotelsDto;
    }

    private static void HideAdminProperities(HotelDto h)
    {
        h.Created = null;
        h.CreatedBy = null;
        h.LastModified = null;
        h.LastModifiedBy = null;
    }
}
