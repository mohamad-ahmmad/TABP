using Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Users endpoints.
/// </summary>
[ApiController]
[Route("/api/users")]
public class UsersController : Controller
{
    //private readonly IMediator _mediator;

    //public UsersController(IMediator mediator)
    //{
    //    _mediator = mediator;
    //}

    ///// <summary>
    ///// Add a user.
    ///// </summary>
    ///// <param name="user">User data to save.</param>
    ///// <param name="cancellationToken"></param>
    ///// <returns>ActionResult</returns>
    ///// <response code="200">Return the added user data.</response>
    ///// <response code="400">List of errors.</response>
    //[HttpPost]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<ActionResult<UserDto>> AddUser(UserForCreateDTO user, CancellationToken cancellationToken)
    //{
    //    var createUserCommand = new CreateUserCommand(user);
    //    var result = await _mediator.Send(createUserCommand, cancellationToken);
        
    //    if (result.IsFailure)
    //    {
    //        return StatusCode((int)result.StatusCode,new { result.Errors });
    //    }

    //    return Ok(result.Response);
    //}

}
