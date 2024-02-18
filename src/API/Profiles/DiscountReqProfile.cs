using API.Models;
using Application.Discounts.Dtos;
using AutoMapper;
#pragma warning disable CS1591

namespace API.Profiles;
public class DiscountReqProfile : Profile
{
    public DiscountReqProfile()
    {
        CreateMap<DiscountRequest, DiscountDto>();
        CreateMap<DiscountDto, DiscountResponse>();
    }
}
