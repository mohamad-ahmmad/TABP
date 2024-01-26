﻿using API.Models;
using Application.Abstractions;
using Application.Hotels.Commands.CreateHotel;
using Application.Hotels.Commands.DeleteHotelById;
using Application.Hotels.Dtos;
using Application.Hotels.Queries.GetHotelById;
using Application.Hotels.Queries.GetHotels;
using AutoMapper;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    [ProducesResponseType(typeof(void) ,StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(HotelResponse) ,StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorsList) ,StatusCodes.Status400BadRequest)]
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
    [ProducesResponseType(typeof(HotelResponse) ,StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void) ,StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<HotelResponse>> GetHotelById(Guid hotelId, CancellationToken cancellationToken)
    {
        var getHotelByIdQuery = new GetHotelByIdQuery(hotelId);
        var result = await _sender.Send(getHotelByIdQuery, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList() { Errors = result.Errors });
        }
        var hotelResponse = MapHotelDtoToHotelResponse(result.Response!);
        hotelResponse.Links.Add(new Link(_linkGenerator.GetPathByName(_httpContextAccessor.HttpContext!, GetHotel, new { })!,
            "self",
            "GET"));
        return Ok(hotelResponse);
    }

    private HotelResponse MapHotelDtoToHotelResponse(HotelDto hotelDto)
    {
        var hotelResponse = _mapper.Map<HotelResponse>(hotelDto);
        hotelResponse.Links.Add(new Link(_linkGenerator.GetPathByName(_httpContextAccessor.HttpContext!, CitiesController.GetCity, new { id = hotelDto.CityId })!,
            "hotel-city",
            "GET"));
        hotelResponse.Links.Add(new Link(_linkGenerator.GetPathByName(_httpContextAccessor.HttpContext!, OwnersController.GetOwner, new { ownerId = hotelDto.OwnerId })!,
            "hotel-owner",
            "GET"));
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
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors = result.Errors});
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
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(void) ,StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(PagedListResponse<HotelResponse>), StatusCodes.Status200OK)]
    [EndpointName(GetHotels)]
    public async Task<ActionResult<PagedListResponse<HotelResponse>>> GetPagedHotels(string? sortCol,
        string? sortOrder,
        string? searchTerm,
        int page=1,
        int pageSize=20)
    {
        if (pageSize <= 0 || pageSize > 20)
        {
            pageSize = 20;
        }

        var getPagedHotelsQuery = new GetHotelsQuery(sortCol, sortOrder, searchTerm, page, pageSize, _userContext.GetUserLevel());

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
        var hotelsResponse = _mapper.Map <IEnumerable<HotelResponse>>(hotelsDto);
        foreach (var item in hotelsResponse)
        {
            item.Links.Add(new Link(_linkGenerator.GetPathByName(_httpContextAccessor.HttpContext!, CitiesController.GetCity, new { id = item.CityId })!,
            "hotel-city",
            "GET"));
            item.Links.Add(new Link(_linkGenerator.GetPathByName(_httpContextAccessor.HttpContext!, OwnersController.GetOwner, new { ownerId = item.OwnerId })!,
                "hotel-owner",
                "GET"));
        }
        return hotelsResponse;
    }
}