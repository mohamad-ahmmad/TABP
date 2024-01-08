using API.Models;
using Application.Owners.Commands.Create;
using Application.Owners.Commands.Delete;
using Application.Owners.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("/api/owners")]
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

    /// <summary>
    /// Delete an owner
    /// </summary>
    /// <param name="ownerId"></param>
    /// <returns></returns>
    /// <response code="400">List of errors.</response>
    /// <response code="204">Successfully deleted.</response>
    /// <response code="404">Owner not found.</response>
    [HttpDelete("{ownerId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorsList) ,StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorsList) ,StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> DeleteOwner(Guid ownerId)
    {
        var deleteOwnerCommand = new DeleteOwnerCommand(ownerId);
        
        var result = await _sender.Send(deleteOwnerCommand);

        if(result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors = result.Errors});
        }
        
        return NoContent();
    }
}
