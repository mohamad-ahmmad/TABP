using API.Models;
using Application.Rooms.Commands.Create;
using Application.Rooms.Commands.DeleteRoomById;
using Application.Rooms.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/api/hotels/{hotelId}/room-infos/{roomInfoId}/rooms")]
public class RoomsController : Controller
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public RoomsController(ISender sender,
        IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }
    /// <summary>
    /// Create a room
    /// </summary>
    /// <param name="roomForCreationRequest"></param>
    /// <param name="hotelId"></param>
    /// <param name="roomInfoId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(RoomDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<RoomDto>> CreateRoom([FromBody]RoomForCreationRequest roomForCreationRequest,
        [FromRoute]Guid hotelId,
        [FromRoute]Guid roomInfoId,
        CancellationToken cancellationToken)
    {
        var roomForCreationDto = _mapper.Map<RoomForCreationDto>(roomForCreationRequest);
        roomForCreationDto.RoomInfoId = roomInfoId;

        var createRoomCommand = new CreateRoomCommand(roomForCreationDto);

        var result = await _sender.Send(createRoomCommand, cancellationToken);

        if(result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors = result.Errors });
        }

        return CreatedAtRoute(new { hotelId, roomInfoId }, result.Response);
    }

    /// <summary>
    /// Delete a room by id
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{roomId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> DeleteRoomById(Guid roomId, CancellationToken cancellationToken)
    {
        var deleteRoomByIdCommand = new DeleteRoomByIdCommand(roomId);

        var result = await _sender.Send(deleteRoomByIdCommand, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, result.Errors);
        }

        return NoContent();
    }
}
