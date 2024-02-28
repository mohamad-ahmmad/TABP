using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.CartItems.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using System.Net;

namespace Application.CartItems.Commands.AddCartItem;
public class AddCartItemCommandHandler : ICommandHandler<AddCartItemCommand, CartItemDto>
{
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICartItemsRepository _cartItemsRepo;

    public AddCartItemCommandHandler(ICartItemsRepository cartItemsRepo,
        IMapper mapper,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
        _cartItemsRepo = cartItemsRepo;
    }

    public async Task<Result<CartItemDto>> Handle(AddCartItemCommand request,
        CancellationToken cancellationToken)
    {
        if (request.CartItemDto.UserId != _userContext.GetUserId())
        {
            return Result<CartItemDto>.Failure(CartItemErrors.ForbidToCreateCartItem,
                HttpStatusCode.Forbidden);
        }
        
        var itemCart = _mapper.Map<CartItem>(request.CartItemDto);
        
        var result = await _cartItemsRepo.AddCartItemAsync(itemCart, cancellationToken);
        if(result.IsFailure)
        {
            return Result<CartItemDto>.Failure(result.Errors.First(), HttpStatusCode.NotFound);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
        
        var itemCartDto = _mapper.Map<CartItemDto>(itemCart);
        
        return Result<CartItemDto>.Success(itemCartDto)!;
    }
}
