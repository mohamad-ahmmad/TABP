using API.Models;
using Application.Owners.Commands.Create;
using Application.Owners.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("/api/Owners")]
public class OwnersController : Controller
{
    private readonly ISender _sender;

    public OwnersController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Create owner
    /// </summary>
    /// <param name="ownerForCreateDto"></param>
    /// <returns>Owner</returns>
    /// <response code="400">List of errors.</response>
    /// <response code="200">The created owner.</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ErrorsList),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(OwnerDto) ,StatusCodes.Status200OK)]
    public async Task<ActionResult<OwnerDto>> AddOwner([FromBody]OwnerForCreateDto ownerForCreateDto) 
    {
        var createOwnerCommand = new CreateOwnerCommand(ownerForCreateDto);
        var result = await _sender.Send(createOwnerCommand);

        if(result.IsFailure) 
        {
            return StatusCode((int)result.StatusCode, new ErrorsList()
            {
                Errors = result.Errors
            });        
        }

        return Ok(result.Response);
    }
}
