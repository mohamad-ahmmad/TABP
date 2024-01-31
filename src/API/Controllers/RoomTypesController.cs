using API.Models;
using Application.RoomTypes.Commands.CreateRoomType;
using Application.RoomTypes.Dtos;
using Application.RoomTypes.Queries.GetAllRoomTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;
[ApiController]
[Route("/api/room-types")]
public class RoomTypesController : Controller
{
    private readonly ISender _sender;

    public RoomTypesController(ISender sender)
    {
        _sender =  sender;
    }

    /// <summary>
    /// Create a room type
    /// </summary>
    /// <param name="roomTypeForCreateDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(RoomTypeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RoomTypeDto>> CreateRoomType(RoomTypeForCreateDto roomTypeForCreateDto, CancellationToken cancellationToken) 
    {
        var createRoomTypeCommand = new CreateRoomTypeCommand(roomTypeForCreateDto);
        var result = await _sender.Send(createRoomTypeCommand, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList(){ Errors=result.Errors});
        }

        return CreatedAtRoute(new {id = result.Response!.Id }, result.Response);
    }


    /// <summary>
    /// Get all room types.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<RoomTypeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<RoomTypeDto>>> GetAllRoomTypes(CancellationToken cancellationToken)
    {
        var getAllRoomTypesQuery = new GetAllRoomTypesQuery();
        var result = await _sender.Send(getAllRoomTypesQuery, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList() { Errors = result.Errors});
        }

        return Ok(result.Response);
    }
}

