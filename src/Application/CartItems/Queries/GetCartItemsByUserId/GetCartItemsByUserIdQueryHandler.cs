using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.CartItems.Dtos;
using AutoMapper;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using System.Net;

namespace Application.CartItems.Queries.GetCartItemsByUserId;
public class GetCartItemsByUserIdQueryHandler : IQueryHandler<GetCartItemsByUserIdQuery, IEnumerable<CartItemDto>>
{
    private readonly ICartItemsRepository _cartItemsRepo;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public GetCartItemsByUserIdQueryHandler(ICartItemsRepository cartItemsRepo,
        IUserContext userContext,
        IMapper mapper)
    {
        _cartItemsRepo = cartItemsRepo;
        _userContext = userContext;
        _mapper = mapper;
    }
    public async Task<Result<IEnumerable<CartItemDto>>> Handle(GetCartItemsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        if(request.UserId != _userContext.GetUserId())
        {
            return Result<IEnumerable<CartItemDto>>.Failure(CartItemErrors.ForbidToGetCartItems, HttpStatusCode.Forbidden);
        }

        var cartItems = await _cartItemsRepo.GetCartItemsByUserIdAsync(request.UserId, cancellationToken);

        var cartItemsDto = _mapper.Map<IEnumerable<CartItemDto>>(cartItems);

        return Result<IEnumerable<CartItemDto>>.Success(cartItemsDto)!;
    }
}

