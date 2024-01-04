using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Cities.Dtos;
using Application.Cities.Mappings;
using Application.Dtos;
using AutoMapper;
using Domain.Enums;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.Extensions.Logging;

namespace Application.Cities.Queries.GetCities;

public class GetCitiesQueryHandler : IQueryHandler<GetCitiesQuery, PagedList<CityDto>>
{
    private readonly ICitiesRepository _citiesRepo;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCitiesQueryHandler> _logger;

    public GetCitiesQueryHandler
        (
            ICitiesRepository citiesRepo,
            IUserContext userContext,
            IMapper mapper,
            ILogger<GetCitiesQueryHandler> logger
        )
    {
        _citiesRepo = citiesRepo;
        _userContext = userContext;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<Result<PagedList<CityDto>>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        var citiesAndCount= await _citiesRepo.GetCitiesAsync(request.SearchTerm,
                                    request.sortCol,
                                    request.sortOrder,
                                    request.page,
                                    request.pageSize,
                                    cancellationToken);
        var (cities, totalCount) = citiesAndCount;

        _logger.LogInformation("The user with '{id}' ID and {role} role\n" +
            "has sucessfully requested {totalCount} cities.", _userContext.GetUserId(), _userContext.GetUserLevel(), totalCount);

        var isAdmin = _userContext.GetUserLevel() == UserLevels.Admin;

        return new PagedList<CityDto>(CityMapper.MapCitiesToCitiesDto(cities, _mapper, isAdmin),
            request.page,
            request.pageSize,
            totalCount);
    }
}

