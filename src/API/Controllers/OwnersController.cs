using API.Models;
using Application.Dtos;
using Application.Owners.Commands.Create;
using Application.Owners.Commands.Delete;
using Application.Owners.Commands.Update;
using Application.Owners.DTOs;
using Application.Owners.Queries.GetOwners;
using Infrastructure.Services.Patch;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
    /// <summary>
    /// Get owners.
    /// </summary>
    /// <param name="searchTerm">Used for firstname and lastname searching</param>
    /// <param name="phoneNumber">Used for phonenumber</param>
    /// <param name="page"></param>
    /// <param name="pageSize">Maximum size is 30</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(PagedList<OwnerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorsList) ,StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorsList) ,StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedList<OwnerDto>>> GetOwners(string? searchTerm,
        string? phoneNumber,
        int page =1,
        int pageSize =30)
    {

        var ownersQuery = new GetOwnersQuery(searchTerm, phoneNumber, page, pageSize);

        var result = await _sender.Send(ownersQuery);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors = result.Errors });
        }

        return Ok(result.Response);
    }
    /// <summary>
    /// Patch a owner
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="patchDocument"></param>
    /// <returns>NoContent</returns>
    [HttpPatch("{ownerId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorsList) ,StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorsList) ,StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> PatchOwner(Guid ownerId, [FromBody]JsonPatchDocument<OwnerDto> patchDocument)
    {
        var patchRequest = new JsonPatchRequest<OwnerDto>(patchDocument);
        var updateOwnerRequest = new UpdateOwnerCommand(ownerId, patchRequest);

        var result = await _sender.Send(updateOwnerRequest);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors = result.Errors });
        }

        return NoContent();
    }
}
