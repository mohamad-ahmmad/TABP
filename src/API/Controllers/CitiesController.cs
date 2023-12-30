using Application.Cities.Commands.Create;
using Application.Cities.Dtos;
using Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers;

[ApiController]
[Route("/api/cities")]
public class CitiesController : Controller
{
    private readonly IMediator _mediator;

    public CitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CityForAdminDto>> CreateCity([FromForm] string cityJson, [FromForm] IFormFile image)
    {
        var city = JsonSerializer.Deserialize<CityForCreateDto>(cityJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        city!.Image = image;
        var createCityReq = new CreateCityCommand(city);
        var result = await _mediator.Send(createCityReq);
        
        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode,new { result.Errors });
            
        }

        return Ok(result.Response);   
    }
}

