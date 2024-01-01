using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Cities.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Cities.Queries.GetCityById;

public class GetCityByIdCommandHandler : IQueryHandler<GetCityByIdCommand, CityDto>
{
    private readonly ICitiesRepository _cityRepo;
    private readonly ILogger<GetCityByIdCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public GetCityByIdCommandHandler(
        ICitiesRepository cityRepo,
        ILogger<GetCityByIdCommandHandler> logger,
        IUserContext userContext,
        IMapper mapper
        )
    {
        _cityRepo = cityRepo;
        _logger = logger;
        _userContext = userContext;
        _mapper = mapper;
    }
    public async Task<Result<CityDto>> Handle(GetCityByIdCommand request,
        CancellationToken cancellationToken)
    {
        var city = await _cityRepo.GetCityByIdAsync(request.CityId, cancellationToken);
        if (city is null)
        {
            return Result<CityDto>.Failure(CityErrors.CityNotFound, HttpStatusCode.NotFound); 
        }
        var isAdmin = _userContext.GetUserLevel() == UserLevels.Admin;
        _logger.LogInformation($"The city with '{request.CityId}' ID is found, the user make is admin : {isAdmin} " +
            $"and has the following ID : {_userContext.GetUserId()}.");

        return MapCityToCityDto(city, isAdmin);
    }

    public CityDto MapCityToCityDto(City city, bool isAdmin)
    {
        var cityDto = _mapper.Map<CityDto>(city);
        cityDto.IsAdmin = isAdmin;
        if(!isAdmin)
        {
            cityDto.Created = null;
            cityDto.CreatedBy = null;
            cityDto.LastModified = null;
            cityDto.LastModifiedBy= null;
        }
        return cityDto;
    }
}

