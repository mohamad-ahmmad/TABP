using API.Models;
using Application.RoomInfos.Commands.Create;
using Application.RoomInfos.Commands.DeleteRoomInfoById;
using Application.RoomInfos.Dtos;
using AutoMapper;
using Azure;
using Domain.Shared;
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
        var roomInfoResponse = GenerateLinks(result.Response!);
        roomInfoResponse.Links.Add(new Link(_linkGenerator.GetPathByName(CreateRoomInfoEndpoint, new {hotelId})!,
       "self",
       "POST"));
        return CreatedAtRoute(new { hotelId, roomInfoId = result.Response!.Id}, roomInfoResponse);
    }

    
    
    private RoomInfoResponse GenerateLinks(RoomInfoDto roomInfoDto)
    {
        var response = _mapper.Map<RoomInfoResponse>(roomInfoDto);
        //Add one for rooms
        //Add one for images
        return response;
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
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors =result.Errors });    
        }

        return NoContent();
    }
}
