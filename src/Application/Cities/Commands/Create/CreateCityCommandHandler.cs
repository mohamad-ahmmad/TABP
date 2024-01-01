using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Cities.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Cities.Commands.Create;

public class CreateCityCommandHandler : ICommandHandler<CreateCityCommand, CityDto>
{
    private readonly ILogger<CreateCityCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICitiesRepository _cityRepo;
    private readonly IImageUploaderService _imageUploader;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public CreateCityCommandHandler(
        ILogger<CreateCityCommandHandler> logger,
        IUnitOfWork unitOfWork,
        ICitiesRepository cityRepo,
        IImageUploaderService imageUploader,
        IUserContext userContext,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _cityRepo = cityRepo;
        _imageUploader = imageUploader;
        _userContext = userContext;
        _mapper = mapper;
    }
    public async Task<Result<CityDto>> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        if (await _cityRepo.DoesCityExistsAsync(request.CityDto.CityName,
                                                request.CityDto.CountryName,
                                                cancellationToken))
        {
            return Result<CityDto>.Failure(CityErrors.CityAlreadyExist, HttpStatusCode.Conflict);
        }

        if (_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return Result<CityDto>.Failure(CityErrors.UnauthorizedToCreateCity, HttpStatusCode.Unauthorized);
        }

        var urls = await _imageUploader.UploadImageAsync(new List<IFormFile>
        {
            request.CityDto.Image!
        });

        //Retrieve the first image cuz the city only have one thumbnail.
        var imageUrl = urls.First();
        _logger.LogInformation("Image uploaded successfully path: " + imageUrl);

        var city = MapCityDtoToCity(request.CityDto, imageUrl);

        await _cityRepo.AddCityAsync(city, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);
        _logger.LogInformation($"City with '{city.Id}' ID has successfully added.");

        var cityDto = _mapper.Map<CityDto>(city);
        cityDto.IsAdmin = true;

        return cityDto;
    }

    public City MapCityDtoToCity(CityForCreateDto cityForCreateDto, string imageUrl)
        => new City
        {
            CityName = cityForCreateDto.CityName,
            CountryName = cityForCreateDto.CountryName,
            ThumbnailUrl = imageUrl
        };
}

