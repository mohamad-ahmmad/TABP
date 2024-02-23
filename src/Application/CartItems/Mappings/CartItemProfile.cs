using Application.CartItems.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.CartItems.Mappings;
public class CartItemProfile : Profile
{
    public CartItemProfile()
    {
        CreateMap<CartItemForCreationDto,  CartItemForCreationDto>();
        CreateMap<CartItem, CartItemDto>()
            .ForMember(cid => cid.Price, opt => opt.MapFrom<PriceResolver>())
            .ForMember(cid => cid.DiscountPercentage, opt => opt.MapFrom<DiscountPercentageResolver>());
    }
}
