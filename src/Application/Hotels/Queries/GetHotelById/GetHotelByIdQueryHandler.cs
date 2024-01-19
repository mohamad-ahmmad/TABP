using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Hotels.Dtos;
using Application.Hotels.Mappings;
using AutoMapper;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using System.Net;

namespace Application.Hotels.Queries.GetHotelById;
public class GetHotelByIdQueryHandler : IQueryHandler<GetHotelByIdQuery, HotelDto>
{
    private readonly IUserContext _userContext;
    private readonly IHotelsRepository _hotelRepo;
    private readonly IMapper _mapper;

    public GetHotelByIdQueryHandler(IUserContext userContext,
        IHotelsRepository hotelRepo,
        IMapper mapper
        )
    {
        _userContext = userContext;
        _hotelRepo = hotelRepo;
        _mapper = mapper;
    }
    public async Task<Result<HotelDto>> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepo.GetHotelByIdAsync(request.HotelId, cancellationToken);
        if( hotel == null )
        {
            return Result<HotelDto>.Failure(HotelErrors.HotelNotFound, HttpStatusCode.NotFound);
        }
        var hotelDto = HotelMappingUtilities.MapHotelToHotelDto(hotel, _userContext.GetUserLevel(), _mapper);
        return hotelDto;
    }
}