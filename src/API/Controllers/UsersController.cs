using Application.Users.Commands.Create;
using Application.Users.DTOs;
using Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

    [ApiController]
[Route("/api/users")]
public class UsersController : Controller
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Add a user.
    /// </summary>
    /// <param name="user">User data to save.</param>
    /// <returns>ActionResult</returns>
    /// <response code=200>Return the added user data.</response>
    /// <response code=400>Return an error that the client did.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> AddUser(UserForCreateDTO user)
    {
        var createUserCommand = new CreateUserCommand(user);
        var result = await _mediator.Send(createUserCommand);

        if (result.IsFailure)
        {
            return BadRequest(new
            {
                result.Errors
            });
        }

        return Ok(result.Response);
    }

}
