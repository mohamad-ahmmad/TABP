using Application.Abstractions.Messaging;
using Application.RoomInfos.Dtos;
using AutoMapper;
using Domain.Repositories;
using Domain.Shared;

namespace Application.RoomInfos.Queries.GetAllRoomInfosByHotelId;
public class GetAllRoomInfosByHoteldQueryHandler : IQueryHandler<GetAllRoomInfosByHotelIdQuery,
    IEnumerable<RoomInfoDto>>
{
    private readonly IRoomInfosRepository _roomInfosRepo;
    private readonly IMapper _mapper;

    public GetAllRoomInfosByHoteldQueryHandler(IRoomInfosRepository roomInfosRepo,
        IMapper mapper)
    {
        _roomInfosRepo = roomInfosRepo;
        _mapper = mapper;
    }
    public async Task<Result<IEnumerable<RoomInfoDto>>> Handle(GetAllRoomInfosByHotelIdQuery request, CancellationToken cancellationToken)
    {
        var roomInfos = await _roomInfosRepo.GetAllRoomInfosAsync(request.HotelId,
            request.RoomType,
            request.MinPrice,
            request.MaxPrice,
            cancellationToken);
        var roomInfosDto = _mapper.Map<IEnumerable<RoomInfoDto>>(roomInfos);
        
        return Result<IEnumerable<RoomInfoDto>>.Success( roomInfosDto)!;
    }
}

