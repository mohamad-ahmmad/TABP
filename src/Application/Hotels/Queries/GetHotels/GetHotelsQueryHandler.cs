using Application.Abstractions.Messaging;
using Application.Dtos;
using Application.Hotels.Dtos;
using Application.Hotels.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Hotels.Queries.GetHotels;
public class GetHotelsQueryHandler : IQueryHandler<GetHotelsQuery, PagedList<HotelDto>>
{
    private readonly IHotelsRepository _hotelRepo;
    private readonly IMapper _mapper;

    public GetHotelsQueryHandler(IHotelsRepository hotelRepo,
        IMapper mapper)
    {
        _hotelRepo = hotelRepo;
        _mapper = mapper;
    }
    public async Task<Result<PagedList<HotelDto>>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
    {
        var pagedHotelsAndTotalNumber = await _hotelRepo.GetHotelsAndTotalCount(
            request.Page,
            request.PageSize,
            request.MinPrice,
            request.MaxPrice,
            request.HotelRating,
            request.Amenities,
            request.HotelType,
            request.RoomType,
            request.SearchTerm,
            request.SortCol,
            request.SortOrder,
            cancellationToken
            );
        (IEnumerable<Hotel> pagedHotels, int totalCount) = pagedHotelsAndTotalNumber;
        var hotelsDtoPagedList = HotelMappingUtilities.MapHotelsToHotelsDto(pagedHotels, request.UserLevel, _mapper);
        var pagedHotelsDto = new PagedList<HotelDto>(hotelsDtoPagedList, request.Page, request.PageSize, totalCount);
        return pagedHotelsDto;
    }
}

