using Application.Discounts.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Discounts.Mappings;
public class DiscountProfile : Profile
{
    public DiscountProfile()
    {
        CreateMap<Discount, DiscountDto>();
        CreateMap<DiscountDto, Discount>()
            .ForMember(d => d.Id, opt => opt.Ignore());
    }
}
