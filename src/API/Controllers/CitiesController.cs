using Application.Cities.Commands.Create;
using Application.Cities.Dtos;
using Domain.Errors;
using Domain.Shared;
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

    /// <summary>
    /// Create a city.
    /// </summary>
    /// <param name="cityJson"></param>
    /// <param name="image"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>City</returns>
    /// <response code="401">Unauthorized to create a city.</response>
    /// <response code="409">List of errors.</response>
    /// <response code="200">The created city.</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CityForAdminDto) ,StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<Error>) ,StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(List<Error>) ,StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CityForAdminDto>> CreateCity([FromForm] string cityJson, [FromForm] IFormFile image, CancellationToken cancellationToken)
    {
        var city = JsonSerializer.Deserialize<CityForCreateDto>(cityJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        city!.Image = image;
        var createCityReq = new CreateCityCommand(city);
        var result = await _mediator.Send(createCityReq, cancellationToken);
        
        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode,new { result.Errors });
            
        }

        return Ok(result.Response);   
    }
}

