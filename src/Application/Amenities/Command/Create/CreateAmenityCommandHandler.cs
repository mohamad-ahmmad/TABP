using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Amenities.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using System.Net;

namespace Application.Amenities.Command.Create;
public class CreateAmenityCommandHandler : ICommandHandler<CreateAmenityCommand, AmenityDto?>
{
    private readonly IAmenitiesRepository _amenitiesRepo;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAmenityCommandHandler(IAmenitiesRepository amenitiesRepo,
        IUserContext userContext,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _amenitiesRepo = amenitiesRepo;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Result<AmenityDto?>> Handle(CreateAmenityCommand request, CancellationToken cancellationToken)
    {
        if (_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return Result<AmenityDto?>.Failure(AmenityErrors.ForbidToCreateAmenity, HttpStatusCode.Forbidden);
        }
        var amenity = _mapper.Map<Amenity>(request.AmenityForCreationDto);

        await _amenitiesRepo.AddAmenityAsync(amenity, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        var amenityDto = _mapper.Map<AmenityDto>(amenity);

        return Result<AmenityDto?>.Success(amenityDto);
    }
}

