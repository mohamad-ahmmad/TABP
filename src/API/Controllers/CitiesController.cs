using API.Models;
using Application.Abstractions;
using Application.Cities.Commands.Create;
using Application.Cities.Dtos;
using Application.Cities.Queries.GetCityById;
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
    private readonly IUserContext _userContext;

    public CitiesController(IMediator mediator, IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
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
    [ProducesResponseType(typeof(CityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CityDto>> CreateCity([FromForm] string cityJson, [FromForm] IFormFile image, CancellationToken cancellationToken)
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
            return StatusCode((int)result.StatusCode, new { result.Errors });

        }

        return Ok(result.Response);
    }

    /// <summary>
    /// Get a city by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">City not found.</response>
    /// <response code="200">Success.</response>
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(CityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CityDto>> GetCityById(Guid id, CancellationToken cancellationToken)
    {
        var request = new GetCityByIdCommand(id, _userContext.GetUserLevel());
        var result = await _mediator.Send(request, cancellationToken);
        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors = result.Errors });
        }

        return Ok(result.Response);
    }
}

