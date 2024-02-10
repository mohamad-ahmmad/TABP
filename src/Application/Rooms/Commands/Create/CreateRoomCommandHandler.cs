using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Rooms.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using System.Net;

namespace Application.Rooms.Commands.Create;
public class CreateRoomCommandHandler : ICommandHandler<CreateRoomCommand, RoomDto?>
{
    private readonly IUserContext _userContext;
    private readonly IRoomsRepository _roomsRepo;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoomCommandHandler(IRoomsRepository roomsRepo,
        IUserContext userContext,
        IMapper mapper,
        IUnitOfWork unitOfWork
        )
    {
        _userContext = userContext;
        _roomsRepo = roomsRepo;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<RoomDto?>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        if(_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return Result<RoomDto?>.Failure(RoomErrors.ForbidToCreateRoom, HttpStatusCode.Forbidden);
        }

        var room = _mapper.Map<Room>(request.RoomForCreationDto);

        await _roomsRepo.AddRoomAsync(room, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);
        var roomDto = _mapper.Map<RoomDto?>(room);
        roomDto!.IsAdmin = true;
        return Result<RoomDto?>.Success(roomDto);
    }
}
