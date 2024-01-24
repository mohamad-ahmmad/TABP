using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using MediatR;
using System.Net;

namespace Application.Hotels.Commands.DeleteHotelById;
public class DeleteHotelByIdCommandHandler : ICommandHandler<DeleteHotelByIdCommand, Unit>
{
    private readonly IHotelsRepository _hotelsRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public DeleteHotelByIdCommandHandler
        (
        IHotelsRepository hotelsRepo,
        IUnitOfWork unitOfWork,
        IUserContext userContext
        )
    {
        _hotelsRepo = hotelsRepo;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }
    public async Task<Result<Unit>> Handle(DeleteHotelByIdCommand request, CancellationToken cancellationToken)
    {
        if(_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return Result<Unit>.Failure(HotelErrors.ForbidToDeleteHotel, HttpStatusCode.Forbidden);
        }
        var isFound = await _hotelsRepo.DeleteHotelByIdAsync(request.HotelId, cancellationToken);
        if(!isFound)
        {
            return Result<Unit>.Failure(HotelErrors.HotelNotFound, HttpStatusCode.NotFound);
        }
        //NOTE: Soft deleted so I should cascade the action to dependant entites.
        await CascadeSoftDelete();
        await _unitOfWork.CommitAsync(cancellationToken);
        return Result<Unit>.Success(HttpStatusCode.NoContent);
    }
    
    private async Task CascadeSoftDelete()
    {
        await Task.Run(() => { });
    }
}

