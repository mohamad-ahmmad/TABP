using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Owners.DTOs;
using AutoMapper;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using FluentValidation;
using MediatR;
using System.Net;

namespace Application.Owners.Commands.Update;
public class UpdateOwnerCommandHandler : ICommandHandler<UpdateOwnerCommand, Unit>
{
    private readonly IOwnersRepository _ownersRepo;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<OwnerDto> _ownerValidator;

    public UpdateOwnerCommandHandler
        (
            IOwnersRepository ownersRepo,
            IUserContext userContext,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<OwnerDto> ownerValidator
        )
    {
        _ownersRepo = ownersRepo;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _ownerValidator = ownerValidator;
    }
    public async Task<Result<Unit>> Handle(UpdateOwnerCommand request, CancellationToken cancellationToken)
    {
        if(_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return Result<Unit>.Failure(OwnerErrors.ForbidToUpdateOwner, HttpStatusCode.Forbidden);
        }
        var owner = await _ownersRepo.GetOwnerByIdAsync(request.OwnerId, cancellationToken);
        if(owner == null)
        {
            return Result<Unit>.Failure(OwnerErrors.OwnerNotFound, HttpStatusCode.NotFound);
        }

        var ownerDto = _mapper.Map<OwnerDto>(owner);

        var ownerDtoPatched = request.Patch.ApplyTo(ownerDto);

        var errors = _ownerValidator.Validate(ownerDtoPatched)
            .Errors
            .Select(e => new Error(e.PropertyName, e.ErrorMessage));
        
        if(errors.Any())
        {
            return Result<Unit>.Failures(errors);
        }

        _mapper.Map(ownerDtoPatched, owner);
        
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<Unit>.Success(HttpStatusCode.NoContent);
    }
}
