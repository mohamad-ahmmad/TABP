using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Owners.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Owners.Commands.Create;
public class CreateOwnerCommandHandler : ICommandHandler<CreateOwnerCommand, OwnerDto>
{
    private readonly ILogger<CreateOwnerCommandHandler> _logger;
    private readonly IOwnersRepository _ownersRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public CreateOwnerCommandHandler(ILogger<CreateOwnerCommandHandler> logger,
        IOwnersRepository ownersRepo,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IMapper mapper)
    {
        _logger = logger;
        _ownersRepo = ownersRepo;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _mapper = mapper;
    }
    public async Task<Result<OwnerDto>> Handle(CreateOwnerCommand request, CancellationToken cancellationToken)
    {
        if (_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return Result<OwnerDto>.Failure(OwnerErrors.ForbidToCreateOwner, HttpStatusCode.Forbidden);
        }
        var owner = _mapper.Map<Owner>(request.OwnerForCreateDto);
        await _ownersRepo.AddOwnerAsync(owner);
        var ownerDto = _mapper.Map<OwnerDto>(owner);
        await _unitOfWork.CommitAsync(cancellationToken);
        return ownerDto;
    }
}

