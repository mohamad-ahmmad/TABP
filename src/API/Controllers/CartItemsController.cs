﻿using API.Models;
using Application.CartItems.Commands.AddCartItem;
using Application.CartItems.Commands.DeleteCartItemById;
using Application.CartItems.Dtos;
using Application.CartItems.Queries.GetCartItemsByUserId;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[ApiController]
[Route("/api/users/{userId}/cart-items")]
public class CartItemsController : Controller
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;
    public const string addCartItem = "add-cart-item";
    public const string getCartItems = "get-cart-items";
    public const string deleteCartItemById = "delete-cart-item-by-id";

    public CartItemsController(ISender sender,
        IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Add item to the user's cart.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cartItem"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="404">The room not found.</response>
    /// <response code="201">Added successfully.</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CartItemResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [EndpointName(addCartItem)]
    public async Task<ActionResult<CartItemResponse>> AddCartItem(Guid userId,
        CartItemRequest cartItem, CancellationToken cancellationToken)
    {
        var cartItemForCreationDto = _mapper.Map<CartItemForCreationDto>(cartItem);
        cartItemForCreationDto.UserId = userId;
        var addCartItemCommand = new AddCartItemCommand(cartItemForCreationDto);

        var result = await _sender.Send(addCartItemCommand,cancellationToken);

        if(result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, result.Errors);
        }

        return CreatedAtRoute(new { userId }, _mapper.Map<CartItemResponse>(result.Response));
    }
    /// <summary>
    /// Get all cart items for a user by user id
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [EndpointName(getCartItems)]
    [ProducesResponseType(typeof(IEnumerable<CartItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<CartItemResponse>>> GetAllCartItemResponsesByUserId(Guid userId,
        CancellationToken cancellationToken)
    {
        var getCartItemsByUserIdQuery = new GetCartItemsByUserIdQuery(userId);

        var result = await _sender.Send(getCartItemsByUserIdQuery, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors = result.Errors });
        }

        return Ok(_mapper.Map<IEnumerable<CartItemResponse>>(result.Response));
    }


    /// <summary>
    /// Delete cart item by id
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cartItemId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{cartItemId}")]
    [Authorize]
    [EndpointName(deleteCartItemById)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]

    public async Task<ActionResult> DeleteCartItemById(Guid userId,
        Guid cartItemId,
        CancellationToken cancellationToken)
    {
        var deleteCartItemCommand = new DeleteCartItemByIdCommand(userId, cartItemId);
        var result = await _sender.Send(deleteCartItemCommand, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors = result.Errors});
        }

        return NoContent();
    }

}
