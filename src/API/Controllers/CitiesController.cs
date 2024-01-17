using API.Models;
using Application.Abstractions;
using Application.Cities.Commands.Create;
using Application.Cities.Commands.Delete;
using Application.Cities.Commands.Update;
using Application.Cities.Dtos;
using Application.Cities.Queries.GetCities;
using Application.Cities.Queries.GetCityById;
using Application.Dtos;
using Infrastructure.Services.Patch;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace API.Controllers;

[ApiController]
[Route("/api/cities")]
public class CitiesController : Controller
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;
    public const string GetCity = "GetCity";
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
    /// <response code="409">The city is already in the system.</response>
    /// <response code="200">The created city.</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CityDto>> CreateCity([FromForm] string cityJson, [FromForm] IFormFile image, CancellationToken cancellationToken)
    {
        //Todo: Extract this to a pipleline
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    OverrideSpecifiedNames = false,
                    ProcessDictionaryKeys = true,
                    ProcessExtensionDataNames = true
                }
            }
        };

        var city = JsonConvert.DeserializeObject<CityForCreateDto>(cityJson, settings);
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
    [EndpointName(GetCity)]
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

    /// <summary>
    /// Get all cities with filtering and pagination options
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="searchTerm">str</param>
    /// <param name="sortCol">longitude | latitude | cityName | countryName</param>
    /// <param name="sortOrder">desc | asc</param>
    /// <param name="page">int</param>
    /// <param name="pageSize">defualt is 20, maximum is 50</param>
    /// <returns>List of cities</returns>
    /// <response code="401">Unauthorized.</response>
    /// <response code="200">Success.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(PagedList<CityDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CityDto>>> GetCities(
        CancellationToken cancellationToken,
        [FromQuery] string? searchTerm,
        [FromQuery] string? sortCol,
        [FromQuery] string? sortOrder,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20
        )
    {
        var citiesQuery = new GetCitiesQuery(searchTerm,
            page,
            pageSize,
            sortCol,
            sortOrder,
            _userContext.GetUserLevel());

        var result = await _mediator.Send(citiesQuery);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, new ErrorsList { Errors = result.Errors });
        }

        return Ok(result.Response);
    }

    /// <summary>
    /// Delete a city.
    /// </summary>
    /// <param name="cityId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="401">Unauthorized</response>
    /// <response code="204">Deleted successfully.</response>
    /// <response code="400">City doesn't exist.</response>
    /// <response code="403">Authenticated but no permission.</response>
    [HttpDelete("{cityId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteCityById(Guid cityId, CancellationToken cancellationToken)
    {
        var deleteCityCommand = new DeleteCityCommand(cityId);
        var result = await _mediator.Send(deleteCityCommand);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode ,new ErrorsList { Errors = result.Errors});
        }
        
        return NoContent();
    }

    /// <summary>
    /// Patch a city.
    /// </summary>
    /// <param name="patch">JsonDocumentPatch</param>
    /// <param name="cityId">City Id</param>
    /// <returns></returns>
    /// <response code="204">Success</response>
    /// <response code="404">City not found</response>
    /// <response code="400">Bad request</response>
    [HttpPatch("{cityId}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorsList), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PatchCity(JsonPatchDocument<CityDto> patch, Guid cityId)
    {
        var patchRequest = new JsonPatchRequest<CityDto>(patch);
        var patchCommand = new UpdateCityCommand(patchRequest, cityId);
        var result = await _mediator.Send(patchCommand);

        if (result.IsFailure)
        {
            return StatusCode((int)result.StatusCode, result.Errors);
        }

        return NoContent();
    }
    
}

