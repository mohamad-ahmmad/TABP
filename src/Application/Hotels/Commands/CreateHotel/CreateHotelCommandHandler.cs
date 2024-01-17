using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Hotels.Dtos;
using Application.Hotels.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Hotels.Commands.CreateHotel;
public class CreateHotelCommandHandler : ICommandHandler<CreateHotelCommand, HotelDto>
{
    private readonly ILogger<CreateHotelCommandHandler> _logger;
    private readonly IHotelsRepository _hotelRepo;
    private readonly IImageUploaderService _imageUploader;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public CreateHotelCommandHandler(
        ILogger<CreateHotelCommandHandler> logger,
        IHotelsRepository hotelRepo,
        IImageUploaderService imageUploader,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IMapper mapper
        )
    {
        _logger = logger;
        _hotelRepo = hotelRepo;
        _imageUploader = imageUploader;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _mapper = mapper;
    }
    public async Task<Result<HotelDto>> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
    {
        if(_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return Result<HotelDto>.Failure(HotelErrors.ForbidToCreateHotel, HttpStatusCode.Forbidden);
        }

        var paths = await _imageUploader.UploadImageAsync(new List<IFormFile>() { request.HotelForCreateDto.ThumbnailImage });
        var thumbnailImageHotelPath = paths.First();

        var hotel = _mapper.Map<Hotel>(request.HotelForCreateDto);
        hotel.ThumbnailUrl = thumbnailImageHotelPath;

        await _hotelRepo.AddHotelAsync(hotel, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        _logger.LogInformation("Admin with '{adminId}' has successfully added hotel with '{hotelId}'."
            ,_userContext.GetUserId(), hotel.Id);
        var hotelDto = HotelMappingUtilities.MapHotelToHotelDto(hotel, _userContext.GetUserLevel(), _mapper);

        return hotelDto;
    }
    
}

