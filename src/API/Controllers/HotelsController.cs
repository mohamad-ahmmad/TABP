using API.Models;
using Application.Hotels.Commands.CreateHotel;
using Application.Hotels.Dtos;
using AutoMapper;
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
        var hotelResponse = _mapper.Map<HotelResponse>(result.Response);
        hotelResponse.Links.Add(new Link(_linkGenerator.GetPathByName(_httpContextAccessor.HttpContext!, CitiesController.GetCity, new { id = result.Response?.CityId })!, "hotel-city", "GET"));
        hotelResponse.Links.Add(new Link(_linkGenerator.GetPathByName(_httpContextAccessor.HttpContext!, HotelsController.AddHotel, new {  })!, "self", "POST"));
        hotelResponse.Links.Add(new Link(_linkGenerator.GetPathByName(_httpContextAccessor.HttpContext!, OwnersController.GetOwner, new { ownerId = result.Response?.OwnerId })!, "hotel-owner", "GET"));
        return CreatedAtRoute(new { id = hotelResponse.Id }, hotelResponse);
    }
}