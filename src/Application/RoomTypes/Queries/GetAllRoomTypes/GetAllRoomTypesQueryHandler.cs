using Application.Abstractions.Messaging;
using Application.RoomTypes.Dtos;
using AutoMapper;
using Domain.Repositories;
using Domain.Shared;

namespace Application.RoomTypes.Queries.GetAllRoomTypes;
public class GetAllRoomTypesQueryHandler : IQueryHandler<GetAllRoomTypesQuery, IEnumerable<RoomTypeDto>?>
{
    private readonly IRoomTypesRepository _roomTypesRepo;
    private readonly IMapper _mapper;

    public GetAllRoomTypesQueryHandler(IRoomTypesRepository roomTypesRepo,
        IMapper mapper)
    {
        _roomTypesRepo = roomTypesRepo;
        _mapper = mapper;
    }
    public async Task<Result<IEnumerable<RoomTypeDto>?>> Handle(GetAllRoomTypesQuery request, CancellationToken cancellationToken)
    {
        var roomTypes = await _roomTypesRepo.GetAllRoomTypesAsync(cancellationToken);
        var roomTypesDto = _mapper.Map<IEnumerable<RoomTypeDto>?>(roomTypes);

        return Result<IEnumerable<RoomTypeDto>?>.Success(roomTypesDto);
    }
}

