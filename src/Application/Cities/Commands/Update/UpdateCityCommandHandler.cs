using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Cities.Dtos;
using AutoMapper;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Cities.Commands.Update;
public class UpdateCityCommandHandler : ICommandHandler<UpdateCityCommand, Unit>
{
    private readonly ICitiesRepository _citiesRepo;
    private readonly ILogger<UpdateCityCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;
    private readonly IValidator<CityDto> _cityValidator;

    public UpdateCityCommandHandler
        ( 
            ICitiesRepository citiesRepo,
            ILogger<UpdateCityCommandHandler> logger,
            IUnitOfWork unitOfWork,
            IUserContext userContext,
            IMapper mapper,
            IValidator<CityDto> cityValidator
        )
    {
        _citiesRepo = citiesRepo;
        _logger= logger;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _mapper = mapper;
        _cityValidator = cityValidator;
    }
    public async Task<Result<Unit>> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {
        if (_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return Result<Unit>.Failure(CityErrors.UnauthorizedToUpdateCity, HttpStatusCode.Forbidden);
        }

        var city = await _citiesRepo.GetCityByIdAsync(request.CityId, cancellationToken);

        if(city == null )
        {
            return Result<Unit>.Failure(CityErrors.CityNotFound, HttpStatusCode.NotFound);
        }

        var cityDto = _mapper.Map<CityDto>(city);

        var cityDtoPatched = request.PatchRequest.ApplyTo(cityDto);

        var errors = _cityValidator.Validate(cityDtoPatched)
            .Errors
            .Select(e=> new Error(e.PropertyName, e.ErrorMessage));
        
        if(errors.Any())
        {
            return Result<Unit>.Failures(errors);
        }
        
        _mapper.Map(cityDtoPatched, city);
        
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Admin with '{id}' ID has update the city with '{id}'.", _userContext.GetUserId(), city.Id);

        return Unit.Value;
    }
}

