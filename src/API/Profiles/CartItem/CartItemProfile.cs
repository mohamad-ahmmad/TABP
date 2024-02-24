using API.Models;
using Application.CartItems.Dtos;
using AutoMapper;

namespace API.Profiles.CartItem;
public class CartItemProfile : Profile
{
    public CartItemProfile()
    {
        CreateMap<CartItemDto, CartItemResponse>()
            .ForMember(cir => cir.Links, opt => opt.MapFrom<CartItemNavigationLinksResolver>());
        CreateMap<CartItemRequest, CartItemForCreationDto>();
    }
}
