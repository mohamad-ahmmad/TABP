using API.Models;
using Application.Bookings.Commands.CheckoutCartItems;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("/api/users/{userId}/bookings")]
public class BookingsController : Controller
{
    private readonly ISender _sender;

    public BookingsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Create bookings by checkout cart items.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="createBookingRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("/api/users/{userId}/cart-items/pay")]
    [Authorize]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> CreateBookingsFromCartItems(Guid userId,
        [FromBody] CreateBookingRequest createBookingRequest,
        CancellationToken cancellationToken)
    {
        var checkOutCartItemsCommand = new CheckoutCartItemsCommand(createBookingRequest.CardDetailsToken,
            createBookingRequest.IdempotencyKey,
            userId);
        
        var result = await _sender.Send(checkOutCartItemsCommand, cancellationToken);

        if(result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors = result.Errors });
        }

        
        return Ok();
    }
}
