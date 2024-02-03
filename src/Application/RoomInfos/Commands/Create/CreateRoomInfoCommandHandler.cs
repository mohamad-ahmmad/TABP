using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.RoomInfos.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using System.Net;

namespace Application.RoomInfos.Commands.Create;
public class CreateRoomInfoCommandHandler : ICommandHandler<CreateRoomInfoCommand, RoomInfoDto?>
{
    private readonly IRoomInfosRepository _roomInfoRepo;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public CreateRoomInfoCommandHandler(IRoomInfosRepository roomInfoRepo,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IMapper mapper)
    {
        _roomInfoRepo = roomInfoRepo;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }
    public async Task<Result<RoomInfoDto?>> Handle(CreateRoomInfoCommand request, CancellationToken cancellationToken)
    {
        if(_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return Result<RoomInfoDto?>.Failure(RoomInfoErrors.ForbidToCreateRoomInfo, HttpStatusCode.Forbidden);
        }

        var roomInfo = _mapper.Map<RoomInfo>(request.RoomInfoForCreateDto);
        
        await _roomInfoRepo.AddRoomInfoAsync(roomInfo, request.HotelId, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        var roomInfoDto = _mapper.Map<RoomInfoDto?>(roomInfo);

        return Result<RoomInfoDto?>.Success(roomInfoDto);
    }
}

