using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using MediatR;
using System.Net;

namespace Application.RoomInfos.Commands.DeleteRoomInfoById;
public class DeleteRoomInfoByIdCommandHandler : ICommandHandler<DeleteRoomInfoByIdCommand, Unit>
{
    private readonly IRoomInfosRepository _roomInfosRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public DeleteRoomInfoByIdCommandHandler(IRoomInfosRepository roomInfosRepo,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _roomInfosRepo = roomInfosRepo; 
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }
    public async Task<Result<Unit>> Handle(DeleteRoomInfoByIdCommand request, CancellationToken cancellationToken)
    {
        if(_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return Result<Unit>.Failure(RoomInfoErrors.ForbidToDeleteRoomInfo, HttpStatusCode.Forbidden);
        }

        var result = await _roomInfosRepo.DeleteRoomInfoByIdAsync(request.RoomInfoId, cancellationToken);
        
        if(result.IsFailure)
        {
            return Result<Unit>.Failure(result.Errors.First(), result.StatusCode);
        }
        
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<Unit>.Success(HttpStatusCode.NoContent);
    }
}

