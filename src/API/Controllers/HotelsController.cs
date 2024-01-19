using API.Models;
using Application.Hotels.Commands.CreateHotel;
using Application.Hotels.Dtos;
using Application.Hotels.Queries.GetHotelById;
using AutoMapper;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers;

[ApiController]
[Route("/api/hotels")]
public class HotelsController : Controller
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISender _sender;
    private readonly JsonSerializerSettings _serializerSettings;
    private readonly IMapper _mapper;
    public const string AddHotel = "CreateHotel";
    public const string GetHotel = "GetHotelById";

    public HotelsController(ISender sender,
        LinkGenerator linkGenerator,
        IHttpContextAccessor httpContextAccessor,
        JsonSerializerSettings serializerSettings,
        IMapper mapper)
    {
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
        _sender = sender;
        _serializerSettings = serializerSettings;
        _mapper = mapper;
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
}