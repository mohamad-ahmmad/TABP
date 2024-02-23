using Application.CartItems.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.CartItems.Mappings;
public class PriceResolver : IValueResolver<CartItem, CartItemDto, double>
{
    public double Resolve(CartItem source, CartItemDto destination, double destMember, ResolutionContext context)
    {
        return source.Room.PricePerDay;
    }
}
