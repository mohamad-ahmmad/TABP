using API.Models;
using Application.RoomInfos.Commands.Create;
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


    [Authorize]
    [HttpPost]
    [EndpointName(CreateRoomInfoEndpoint)]
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
}
