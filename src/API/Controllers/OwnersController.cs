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

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<OwnerDto>> AddOwner([FromBody]OwnerForCreateDto ownerForCreateDto) 
    {
        var createOwnerCommand = new CreateOwnerCommand(ownerForCreateDto);
        var result = await _sender.Send(createOwnerCommand);

        if(result.IsFailure) 
        {
            return StatusCode((int)result.StatusCode, result.Errors);        
        }

        return Ok(result.Response);
    }
}
