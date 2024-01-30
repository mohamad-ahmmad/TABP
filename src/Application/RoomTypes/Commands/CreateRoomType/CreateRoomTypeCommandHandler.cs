using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.RoomTypes.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using System.Net;

namespace Application.RoomTypes.Commands.CreateRoomType;
public class CreateRoomTypeCommandHandler : ICommandHandler<CreateRoomTypeCommand, RoomTypeDto>
{
    private readonly IMapper _mapper;
    private readonly IRoomTypesRepository _roomTypesRepo;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoomTypeCommandHandler(IMapper mapper,
        IRoomTypesRepository roomTypesRepo,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _roomTypesRepo = roomTypesRepo;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<RoomTypeDto>> Handle(CreateRoomTypeCommand request, CancellationToken cancellationToken)
    {
        if(_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return Result<RoomTypeDto>.Failure(RoomTypeErrors.ForbidToCreateRoomType, HttpStatusCode.Forbidden);
        }

        var roomType = _mapper.Map<RoomType>(request.RoomTypeForCreateDto);
        await _roomTypesRepo.AddRoomTypeAsync(roomType, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return _mapper.Map<RoomTypeDto>(roomType);
    }
}

