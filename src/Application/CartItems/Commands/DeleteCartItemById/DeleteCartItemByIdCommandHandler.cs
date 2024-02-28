using Application.Abstractions;
using Application.Abstractions.Messaging;
using AutoMapper;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using System.Net;

namespace Application.CartItems.Commands.DeleteCartItemById;
public class DeleteCartItemByIdCommandHandler : ICommandHandler< DeleteCartItemByIdCommand, Empty>
{
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICartItemsRepository _cartItemsRepo;

    public DeleteCartItemByIdCommandHandler(IUserContext userContext,
        IUnitOfWork unitOfWork,
        ICartItemsRepository cartItemsRepo
        )
    {
        _userContext = userContext;
        _unitOfWork = unitOfWork;
        _cartItemsRepo = cartItemsRepo;
    }
    public async Task<Result<Empty>> Handle(DeleteCartItemByIdCommand request, CancellationToken cancellationToken)
    {

        if(_userContext.GetUserId() == request.UserId)
        {
            return Result<Empty>.Failure(CartItemErrors.ForbidToDeleteCartItem, HttpStatusCode.Forbidden);
        }

        var result = await _cartItemsRepo.DeleteCartItemByIdAsync(request.CartItemId, request.UserId, cancellationToken);

        if (result.IsFailure)
        {
            return result;
        }
        
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<Empty>.Success(HttpStatusCode.NoContent)!;
    }
}
