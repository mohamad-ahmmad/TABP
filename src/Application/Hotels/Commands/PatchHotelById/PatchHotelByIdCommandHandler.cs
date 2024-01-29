using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Hotels.Dtos;
using AutoMapper;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using FluentValidation;
using MediatR;
using System.Net;

namespace Application.Hotels.Commands.PatchHotelById;
public class PatchHotelByIdCommandHandler : ICommandHandler<PatchHotelByIdCommand, Unit>
{
    private readonly IUserContext _userContext;
    private readonly IHotelsRepository _hotelRepo;
    private readonly IMapper _mapper;
    private readonly IValidator<HotelDto> _hotelDtoValidator;
    private readonly IUnitOfWork _unitOfWork;

    public PatchHotelByIdCommandHandler(IHotelsRepository hotelRepo,
        IUserContext userContext,
        IMapper mapper,
        IValidator<HotelDto> hotelDtoValidator,
        IUnitOfWork unitOfWork
        )
    {
        _userContext = userContext;
        _hotelRepo = hotelRepo;
        _mapper = mapper;
        _hotelDtoValidator = hotelDtoValidator;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Unit>> Handle(PatchHotelByIdCommand request, CancellationToken cancellationToken)
    {
        if (_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return HotelErrors.ForbidToCreateHotel;
        }
        var hotel = await _hotelRepo.GetHotelByIdAsync(request.HotelId, cancellationToken);

        if (hotel == null)
        {
            return HotelErrors.HotelNotFound;
        }

        var hotelDto = _mapper.Map<HotelDto>(hotel);
        var hotelDtoPatched = request.PatchRequest.ApplyTo(hotelDto);
        var errors = _hotelDtoValidator.Validate(hotelDtoPatched)
            .Errors
            .Select(e => new Error(e.PropertyName, e.ErrorMessage));
        if (errors.Any())
        {
            return Result<Unit>.Failures(errors);
        }
        _mapper.Map(hotelDtoPatched, hotel);

        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<Unit>.Success(HttpStatusCode.NoContent);
    }
}

