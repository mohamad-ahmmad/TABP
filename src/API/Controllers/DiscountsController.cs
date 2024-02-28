using API.Models;
using Application.Discounts.Command.Create;
using Application.Discounts.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Controllers;

/// <summary>
/// 
/// </summary>

[ApiController]
[Route("/api/hotels/{hotelId}/room-infos/{roomInfoId}/rooms/{roomId}/discounts")]
public class DiscountsController : Controller
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;
    public const string AddDiscountEndpoint= "AddDiscount";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="mapper"></param>
    public DiscountsController(ISender sender,
        IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Create discount for a room
    /// </summary>
    /// <param name="discountReq"></param>
    /// <param name="roomId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(DiscountDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status400BadRequest)]
    [EndpointName(nameof(AddDiscountEndpoint))]
    public async Task<ActionResult<DiscountResponse>> AddDiscount(DiscountRequest discountReq,
        Guid roomId,
        Guid hotelId,
        Guid roomInfoId,
        CancellationToken cancellationToken) 
    {
        discountReq.ToDate = discountReq.ToDate.Date;
        discountReq.FromDate = discountReq.FromDate.Date;
        
        var discountDto = _mapper.Map<DiscountDto>(discountReq);
        discountDto.RoomId = roomId;

        var createDiscountCommand = new CreateDiscountCommand(discountDto);

        var result = await _sender.Send(createDiscountCommand, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors = result.Errors });
        }

        var discountResponse = _mapper.Map<DiscountResponse>(result.Response);
        return CreatedAtRoute(new { discountId = result.Response!.Id }, discountResponse);
    } 
}
