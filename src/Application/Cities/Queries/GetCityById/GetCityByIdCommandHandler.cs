using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Cities.Dtos;
using Application.Cities.Mappings;
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

        _logger.LogInformation("The city with '{CityId}' ID is found, the user rule is {Role} " +
            "and has the following ID : '{UserId}'.", request.CityId, isAdmin ? "Admin" : "User", _userContext.GetUserId());

        return CityMapper.MapCityToCityDto(city, _mapper, isAdmin);
    }

   
}

