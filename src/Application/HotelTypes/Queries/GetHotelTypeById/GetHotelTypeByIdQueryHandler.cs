using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.HotelTypes.Dtos;
using AutoMapper;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using System.Net;

namespace Application.HotelTypes.Queries.GetHotelTypeById;
public class GetHotelTypeByIdQueryHandler : IQueryHandler<GetHotelTypeByIdQuery, HotelTypeDto>
{
    private readonly IHotelTypesRepository _hotelTypesRepo;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public GetHotelTypeByIdQueryHandler(IHotelTypesRepository hotelTypesRepo,
        IUserContext userContext,
        IMapper mapper)
    {
        _hotelTypesRepo = hotelTypesRepo;
        _userContext = userContext;
        _mapper = mapper;
    }
    public async Task<Result<HotelTypeDto>> Handle(GetHotelTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var hotelType = await _hotelTypesRepo.GetHotelTypeByIdAsync(request.HotelTypeId, cancellationToken);
        if (hotelType == null)
        {
            return Result<HotelTypeDto>.Failure(HotelTypeErrors.HotelTypeNotFound, HttpStatusCode.NotFound);
        }
        var hotelTypeDto = _mapper.Map<HotelTypeDto>(hotelType);
        return hotelTypeDto!;
    }
}

