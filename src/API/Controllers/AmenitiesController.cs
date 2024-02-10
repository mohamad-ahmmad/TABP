using API.Models;
using Application.Amenities.Command.Create;
using Application.Amenities.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("/api/hotels/{hotelId}/amenities")]
public class AmenitiesController : Controller
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public AmenitiesController(ISender sender,
        IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Create an amenity
    /// </summary>
    /// <param name="hotelId"></param>
    /// <param name="amenityForCreationRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AmenityDto>> CreateAmenity(Guid hotelId,
        AmenityForCreationRequest amenityForCreationRequest,
        CancellationToken cancellationToken)
    {
        var amenityForCreationDto = _mapper.Map<AmenityForCreationDto>(amenityForCreationRequest);
        amenityForCreationDto.HotelId = hotelId;

        var createAmenityCommand = new CreateAmenityCommand(amenityForCreationDto);

        var result = await _sender.Send(createAmenityCommand, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors = result.Errors });
        }

        return CreatedAtRoute(new {hotelId, amenityId =result.Response!.Id}, result.Response);   
    } 

}
