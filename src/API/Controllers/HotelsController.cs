using API.Models;
using Application.Abstractions;
using Application.Hotels.Commands.CreateHotel;
using Application.Hotels.Commands.DeleteHotelById;
using Application.Hotels.Commands.PatchHotelById;
using Application.Hotels.Dtos;
using Application.Hotels.Queries.GetHotelById;
using Application.Hotels.Queries.GetHotels;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Services.Patch;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;

namespace API.Controllers;

/// <summary>
/// Hotel endpoints
/// </summary>
[ApiController]
[Route("/api/hotels")]
public class HotelsController : Controller
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISender _sender;
    private readonly JsonSerializerSettings _serializerSettings;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    public const string AddHotel = "CreateHotel";
    public const string GetHotel = "GetHotelById";
    public const string GetHotels = "GetHotels";

    public HotelsController(ISender sender,
        LinkGenerator linkGenerator,
        IHttpContextAccessor httpContextAccessor,
        JsonSerializerSettings serializerSettings,
        IMapper mapper,
        IUserContext userContext)
    {
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
        _sender = sender;
        _serializerSettings = serializerSettings;
        _mapper = mapper;
        _userContext = userContext;
    }

    /// <summary>
    /// Create a hotel
    /// </summary>
    /// <param name="hotelForCreateDto"></param>
    /// <param name="thumbnail"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(HotelResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status400BadRequest)]
    [EndpointName(AddHotel)]
    public async Task<ActionResult<HotelResponse>> CreateHotel([FromForm] string hotelForCreateDto, [FromForm] IFormFile thumbnail, CancellationToken cancellationToken)
    {
        var hotel = JsonConvert.DeserializeObject<HotelForCreateDto>(hotelForCreateDto, _serializerSettings);
        hotel!.ThumbnailImage = thumbnail;
        var createHotelCommand = new CreateHotelCommand(hotel);
        var result = await _sender.Send(createHotelCommand, cancellationToken);
        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList() { Errors = result.Errors });
        }
        var hotelResponse = MapHotelDtoToHotelResponse(result.Response!);
        hotelResponse.Links.Add(new Link(_linkGenerator.GetPathByName(_httpContextAccessor.HttpContext!, HotelsController.AddHotel, new { })!, "self", "POST"));
        return CreatedAtRoute(new { id = hotelResponse.Id }, hotelResponse);
    }

    /// <summary>
    /// Get hotel by id
    /// </summary>
    /// <param name="hotelId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{hotelId}")]
    [Authorize]
    [EndpointName(GetHotel)]
    [ProducesResponseType(typeof(HotelResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<HotelResponse>> GetHotelById(Guid hotelId, CancellationToken cancellationToken)
    {
        var getHotelByIdQuery = new GetHotelByIdQuery(hotelId);
        var result = await _sender.Send(getHotelByIdQuery, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList() { Errors = result.Errors });
        }
        var hotelResponse = MapHotelDtoToHotelResponse(result.Response!);
        hotelResponse.Links.Add(new Link(_linkGenerator.GetPathByName(_httpContextAccessor.HttpContext!, GetHotel, new {hotelId})!,
            "self",
            "GET"));
        return Ok(hotelResponse);
    }

    private HotelResponse MapHotelDtoToHotelResponse(HotelDto hotelDto)
    {
        var hotelResponse = _mapper.Map<HotelResponse>(hotelDto);
        AddCommonLinks(hotelResponse);
        return hotelResponse;
    }

    /// <summary>
    /// Delete hotel with specific id.
    /// </summary>
    /// <param name="hotelId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("{hotelId}")]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> DeleteHotelById(Guid hotelId, CancellationToken cancellationToken)
    {
        var deleteHotelByIdCommand = new DeleteHotelByIdCommand(hotelId);
        var result = await _sender.Send(deleteHotelByIdCommand, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors = result.Errors });
        }

        return NoContent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sortCol">hotelname | numberofrooms | streetname | longitude | latitude </param>
    /// <param name="sortOrder">asc | desc</param>
    /// <param name="searchTerm">Used for street name and hotel name</param>
    /// <param name="page"></param>
    /// <param name="pageSize">max is 20</param>
    /// <param name="amenities"></param>
    /// <param name="hotelRating">hotel rating like: 4.5 3.43</param>
    /// <param name="hotelType">Boutique | ....</param>
    /// <param name="maxPrice"></param>
    /// <param name="minPrice"></param>
    /// <param name="roomType"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(PagedListResponse<HotelResponse>), StatusCodes.Status200OK)]
    [EndpointName(GetHotels)]
    public async Task<ActionResult<PagedListResponse<HotelResponse>>> GetPagedHotels(int? minPrice,
        int? maxPrice,
        double? hotelRating,
        string? amenities,
        string? hotelType,
        string? roomType,
        string? sortCol,
        string? sortOrder,
        string? searchTerm,
        int page = 1,
        int pageSize = 20)
    {
        //Move it to pipline 
        if (pageSize <= 0 || pageSize > 20)
        {
            pageSize = 20;
        }

        var getPagedHotelsQuery = new GetHotelsQuery(
            minPrice,
            maxPrice,
            hotelRating,
            amenities,
            hotelType,
            roomType,
            sortCol,
            sortOrder,
            searchTerm, page,
            pageSize,
            _userContext.GetUserLevel());

        var result = await _sender.Send(getPagedHotelsQuery);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors = result.Errors });
        }

        var hotelsResponse = MapHotelsDtoToHotelsResponse(result.Response!.Data);

        var pagedHotelsResponse = new PagedListResponse<HotelResponse>(hotelsResponse,
            page,
            pageSize,
            result.Response.TotalCount);

        AddPaginatedLinks(sortCol,
            sortOrder,
            searchTerm,
            page,
            pageSize,
            pagedHotelsResponse);

        return Ok(pagedHotelsResponse);
    }

    private void AddPaginatedLinks(string? sortCol,
        string? sortOrder,
        string? searchTerm,
        int page,
        int pageSize,
        PagedListResponse<HotelResponse> pagedHotelsResponse)
    {
        if (pagedHotelsResponse.HasNextPage)
        {
            pagedHotelsResponse.Links.Add(new Link(
            _linkGenerator
            .GetPathByName
            (
                _httpContextAccessor.HttpContext!,
                HotelsController.GetHotels,
                new { page = page + 1, pageSize, sortCol, sortOrder, searchTerm }!
            )!,
            "next-page",
            "GET"
            ));
        }
        if (pagedHotelsResponse.HasPreviousPage)
        {
            pagedHotelsResponse.Links.Add(new Link(
            _linkGenerator
            .GetPathByName
            (
                _httpContextAccessor.HttpContext!,
                HotelsController.GetHotels,
                new { page = page - 1, pageSize, sortCol, sortOrder, searchTerm }!
            )!,
            "previous-page",
            "GET"
            ));

        }
    }

    private IEnumerable<HotelResponse> MapHotelsDtoToHotelsResponse(IEnumerable<HotelDto> hotelsDto)
    {
        var hotelsResponse = _mapper.Map<IEnumerable<HotelResponse>>(hotelsDto);
        foreach (var item in hotelsResponse)
        {
            AddCommonLinks(item);
        }
        return hotelsResponse;
    }

    private void AddCommonLinks(HotelResponse hotelResponse)
    {
        hotelResponse.Links.Add(new Link(_linkGenerator.GetPathByName(_httpContextAccessor.HttpContext!, CitiesController.GetCity, new { id = hotelResponse.CityId })!,
        "hotel-city",
        "GET"));
        hotelResponse.Links.Add(new Link(_linkGenerator.GetPathByName(_httpContextAccessor.HttpContext!, OwnersController.GetOwner, new { ownerId = hotelResponse.OwnerId })!,
        "hotel-owner",
        "GET"));
    }
    /// <summary>
    /// Patch hotel by hotel's ID.
    /// </summary>
    /// <param name="hotelId"></param>
    /// <param name="jsonPatch"></param>
    /// <returns></returns>
    [HttpPatch("{hotelId}")]
    [Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> PatchHotelById(Guid hotelId, JsonPatchDocument<HotelDto> jsonPatch)
    {
        var patchHotelCommand = new PatchHotelByIdCommand(hotelId, new JsonPatchRequest<HotelDto>(jsonPatch));
        var result = await _sender.Send(patchHotelCommand);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList {Errors = result.Errors});    
        }
        
        return NoContent();
    }
    
}