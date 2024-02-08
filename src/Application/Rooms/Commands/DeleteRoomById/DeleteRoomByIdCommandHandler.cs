using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using MediatR;
using System.Net;

namespace Application.Rooms.Commands.DeleteRoomById;
public class DeleteRoomByIdCommandHandler : ICommandHandler<DeleteRoomByIdCommand, Unit>
{
    private readonly IUserContext _userContext;
    private readonly IRoomsRepository _roomsRepo;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoomByIdCommandHandler(IRoomsRepository roomsRepo,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _roomsRepo = roomsRepo;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Unit>> Handle(DeleteRoomByIdCommand request, CancellationToken cancellationToken)
    {
        if (_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return Result<Unit>.Failure(RoomErrors.ForbidToDeleteRoom, HttpStatusCode.Forbidden);
        }
        var result = await _roomsRepo.DeleteRoomByIdAsync(request.RoomId, cancellationToken);

        if(result.IsFailure)
        {
            return Result<Unit>.Failure(result.Errors.FirstOrDefault()!, result.StatusCode);
        }
        await _unitOfWork.CommitAsync(cancellationToken);
        
        return Result<Unit>.Success(HttpStatusCode.NoContent);
    }
}
