using API.Models;
using Application.HotelTypes.Commands.Create;
using Application.HotelTypes.Dtos;
using Application.HotelTypes.Queries.GetHotelTypeById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("/api/hotel-types")]
public class HotelTypesController : Controller
{
    private readonly ISender _sender;

    public HotelTypesController(ISender sender)
    {
        _sender = sender;
    }


    /// <summary>
    /// Create a hotel type.
    /// </summary>
    /// <param name="hotelTypeDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(HotelTypeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status403Forbidden)]


    public async Task<ActionResult<HotelTypeDto>> AddHotelType([FromBody] HotelTypeDto hotelTypeDto, CancellationToken cancellationToken)
    {
        var createHotelTypeCommand = new CreateHotelTypeCommand(hotelTypeDto);

        var result = await _sender.Send(createHotelTypeCommand, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors = result.Errors });
        }

        return CreatedAtRoute(new {id = result.Response!.Id}, result.Response);
    }

    /// <summary>
    /// Get hotel type by id
    /// </summary>
    /// <param name="hotelTypeId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{hotelTypeId}")]
    public async Task<ActionResult<HotelTypeDto>> GetHotelTypeById(Guid hotelTypeId, CancellationToken cancellationToken)
    {
        var getHotelTypeByIdQuery = new GetHotelTypeByIdQuery(hotelTypeId);
        var result = await _sender.Send(getHotelTypeByIdQuery, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, result.Response);
        }

        return Ok(result.Response);
    }
}

