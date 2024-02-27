using API.Controllers;
using API.Models;
using Application.CartItems.Dtos;
using AutoMapper;

namespace API.Profiles.CartItem;
public class CartItemNavigationLinksResolver : IValueResolver<CartItemDto, CartItemResponse, IEnumerable<Link>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly LinkGenerator _linkGenerator;

    public CartItemNavigationLinksResolver(IHttpContextAccessor httpContextAccessor,
        LinkGenerator linkGenerator)
    {
        _httpContextAccessor = httpContextAccessor;
        _linkGenerator = linkGenerator;
        
    }
    public IEnumerable<Link> Resolve(CartItemDto source, CartItemResponse destination, IEnumerable<Link> destMember, ResolutionContext context)
    {
        HttpContext httpContext = _httpContextAccessor.HttpContext!;
        if (httpContext == null)
        {
            return Enumerable.Empty<Link>();
        }

        var currentEndpointName = httpContext
            .GetEndpoint()?
            .Metadata
            .GetMetadata<IEndpointNameMetadata>()?
            .EndpointName;

        var addCartItemRel = CartItemsController.addCartItem == currentEndpointName ?
            "self" : CartItemsController.addCartItem;

        var getCartItemsRel = CartItemsController.getCartItems == currentEndpointName ?
            "self" : CartItemsController.getCartItems;


        return new List<Link>
        {
            new
            ( _linkGenerator.GetPathByName(CartItemsController.addCartItem, new { userId = source.UserId })!,
                addCartItemRel,
                "POST"
            ),
            new
            (
            _linkGenerator.GetPathByName(CartItemsController.getCartItems, new { userId = source.UserId })!,
            getCartItemsRel,
            "GET"
            )
        };
    }
}
