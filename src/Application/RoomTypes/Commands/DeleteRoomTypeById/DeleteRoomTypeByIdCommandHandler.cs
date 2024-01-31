using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using MediatR;
using System.Net;

namespace Application.RoomTypes.Commands.DeleteRoomTypeById;
public class DeleteRoomTypeByIdCommandHandler : ICommandHandler<DeleteRoomTypeByIdCommand, Unit>
{
    private readonly IRoomTypesRepository _roomTypesRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public DeleteRoomTypeByIdCommandHandler(IRoomTypesRepository roomTypesRepo,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _roomTypesRepo = roomTypesRepo;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }
    public async Task<Result<Unit>> Handle(DeleteRoomTypeByIdCommand request, CancellationToken cancellationToken)
    {
        if(_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return Result<Unit>.Failure(RoomTypeErrors.DeleteRoomType, HttpStatusCode.Forbidden);
        }

        var result = await _roomTypesRepo.DeleteRoomTypeByIdAsync(request.RoomTypeId, cancellationToken);

        if (result.IsFailure)
        {
            return Result<Unit>.Failures(result.Errors, result.StatusCode);
        }
        await _unitOfWork.CommitAsync(cancellationToken);
        return Result<Unit>.Success(HttpStatusCode.NoContent);
    }
}
