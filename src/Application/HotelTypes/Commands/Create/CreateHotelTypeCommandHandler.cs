using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.HotelTypes.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using System.Net;

namespace Application.HotelTypes.Commands.Create;
public class CreateHotelTypeCommandHandler : ICommandHandler<CreateHotelTypeCommand, HotelTypeDto>
{
    private readonly IHotelTypesRepository _hotelTypesRepo;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateHotelTypeCommandHandler(IHotelTypesRepository hotelTypesRepo,
        IUserContext userContext,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _hotelTypesRepo = hotelTypesRepo;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Result<HotelTypeDto>> Handle(CreateHotelTypeCommand request, CancellationToken cancellationToken)
    {
        if (_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return Result<HotelTypeDto>.Failure(HotelTypeErrors.ForbidToCreateHotelType, HttpStatusCode.Forbidden);
        }

        var hotelType = _mapper.Map<HotelType>(request.HotelTypeDto);
        await _hotelTypesRepo.AddHotelTypeAsync(hotelType, cancellationToken);
        var hotelTypeDto = _mapper.Map<HotelTypeDto>(hotelType);
        await _unitOfWork.CommitAsync(cancellationToken);
        
        return hotelTypeDto;
    }
}

