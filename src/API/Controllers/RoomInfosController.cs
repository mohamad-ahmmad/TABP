using API.Models;
using Application.RoomInfos.Commands.Create;
using Application.RoomInfos.Commands.DeleteRoomInfoById;
using Application.RoomInfos.Dtos;
using Application.RoomInfos.Queries.GetAllRoomInfosByHotelId;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("/api/hotels/{hotelId}/room-infos")]
public class RoomInfosController : Controller
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;
    private readonly LinkGenerator _linkGenerator;
    public const string CreateRoomInfoEndpoint = nameof(CreateRoomInfoEndpoint);
    public const string GetAllRoomInfoForHotel = nameof(GetAllRoomInfoForHotel);

    public RoomInfosController(ISender sender,
        IMapper mapper,
        LinkGenerator linkGenerator)
    {
        _sender = sender;
        _mapper = mapper;
        _linkGenerator = linkGenerator;
    }

    /// <summary>
    /// Add room info for a specific hotel by hotel id.
    /// </summary>
    /// <param name="hotelId"></param>
    /// <param name="roomInfoForCreateDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [EndpointName(CreateRoomInfoEndpoint)]
    [ProducesResponseType(typeof(RoomInfoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<RoomInfoResponse>> CreateRoomInfo([FromRoute] Guid hotelId,
        [FromBody] RoomInfoForCreateDto roomInfoForCreateDto,
        CancellationToken cancellationToken)
    {
        var createRoomInfoCommand = new CreateRoomInfoCommand(roomInfoForCreateDto, hotelId);
        var result = await _sender.Send(createRoomInfoCommand, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors = result.Errors });
        }
        var roomInfoResponse = _mapper.Map<RoomInfoResponse>(result.Response!);
        GenerateLinks(roomInfoResponse);
        roomInfoResponse.Links.Add(new Link(_linkGenerator.GetPathByName(CreateRoomInfoEndpoint, new { hotelId })!,
       "self",
       "POST"));
        return CreatedAtRoute(new { hotelId, roomInfoId = result.Response!.Id }, roomInfoResponse);
    }

    /// <summary>
    /// Get all room-infos for a hotel.
    /// </summary>
    /// <param name="hotelId"></param>
    /// <param name="maxPrice"></param>
    /// <param name="minPrice"></param>
    /// <param name="roomType"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [EndpointName(GetAllRoomInfoForHotel)]
    [ProducesResponseType(typeof(IEnumerable<RoomInfoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    
    public async Task<ActionResult<IEnumerable<RoomInfoResponse>>> GetRoomInfosForHotel(Guid hotelId,
        string? roomType,
        int? minPrice,
        int? maxPrice,
    CancellationToken cancellationToken)
    {
        var getAllRoomInfosByHotelIdQuery = new GetAllRoomInfosByHotelIdQuery(hotelId,
            roomType,
            minPrice,
            maxPrice);

        var result = await _sender.Send(getAllRoomInfosByHotelIdQuery, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors = result.Errors });
        }

        var roomInfosResponse = _mapper.Map<IEnumerable<RoomInfoResponse>>(result.Response!);

        foreach (var item in roomInfosResponse)
        {
            GenerateLinks(item);
            item.Links.Add(new Link(_linkGenerator.GetPathByName(GetAllRoomInfoForHotel, new { hotelId })!,
                "self",
                "GET"));
        }

        return Ok(roomInfosResponse);
    }


    private RoomInfoResponse GenerateLinks(RoomInfoResponse roomInfoResponse)
    {
        //Add one for rooms
        //Add one for images
        return roomInfoResponse;
    }

    /// <summary>
    /// Delete room info by id.
    /// </summary>
    /// <param name="roomInfoId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{roomInfoId}")]
    [Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteRoomInfoById(Guid roomInfoId, CancellationToken cancellationToken)
    {
        var deleteRoomInfoByIdCommand = new DeleteRoomInfoByIdCommand(roomInfoId);

        var result = await _sender.Send(deleteRoomInfoByIdCommand, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors = result.Errors });
        }

        return NoContent();
    }


}
