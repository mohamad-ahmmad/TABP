using Application.CartItems.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.CartItems.Mappings;
public class CartItemProfile : Profile
{
    public CartItemProfile()
    {
        CreateMap<CartItemForCreationDto,  CartItem>();
        CreateMap<CartItem, CartItemDto>()
            .ForMember(cid => cid.PricePerDay, opt => opt.MapFrom<PriceResolver>())
            .ForMember(cid => cid.DiscountPercentage, opt => opt.MapFrom<DiscountPercentageResolver>());
    }
}
