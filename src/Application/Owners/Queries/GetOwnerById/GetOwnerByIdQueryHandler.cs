using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Owners.DTOs;
using AutoMapper;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using System.Net;

namespace Application.Owners.Queries.GetOwnerById;
public class GetOwnerByIdQueryHandler : IQueryHandler<GetOwnerByIdQuery, OwnerDto?>
{
    private readonly IOwnersRepository _ownersRepo;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public GetOwnerByIdQueryHandler(IOwnersRepository ownersRepo, IUserContext userContext, IMapper mapper)
    {
        _ownersRepo = ownersRepo;
        _userContext = userContext;
        _mapper = mapper;
    }
    public async Task<Result<OwnerDto?>> Handle(GetOwnerByIdQuery request, CancellationToken cancellationToken)
    {
        if (_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return Result<OwnerDto?>.Failure(OwnerErrors.ForbidToReadOwner, HttpStatusCode.Forbidden);
        }
        var owner = await _ownersRepo.GetOwnerByIdAsync(request.OwnerId, cancellationToken);
        if (owner == null)
        {
            return Result<OwnerDto?>.Failure(OwnerErrors.OwnerNotFound, HttpStatusCode.NotFound);
        }

        var ownerDto = _mapper.Map<OwnerDto?>(owner);

        return ownerDto;
    }
}

