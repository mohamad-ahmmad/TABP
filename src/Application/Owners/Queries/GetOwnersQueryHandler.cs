using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Dtos;
using Application.Owners.DTOs;
using AutoMapper;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using System.Net;

namespace Application.Owners.Queries;
public class GetOwnersQueryHandler : IQueryHandler<GetOwnersQuery, PagedList<OwnerDto>>
{
    private readonly IOwnersRepository _ownersRepo;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public GetOwnersQueryHandler(
        IOwnersRepository ownersRepo,
        IUserContext userContext,
        IMapper mapper
        )
    {
        _ownersRepo = ownersRepo;
        _userContext = userContext;
        _mapper = mapper;
    }
    public async Task<Result<PagedList<OwnerDto>>> Handle(GetOwnersQuery request, CancellationToken cancellationToken)
    {
        if(_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return Result<PagedList<OwnerDto>>.Failure(OwnerErrors.ForbidToReadOwner, HttpStatusCode.Forbidden);
        }

        var ownersAndTotalQueryCounts = await _ownersRepo.GetOwnersAsync(request.Page,
            request.PageSize,
            request.SearchTerm,
            request.PhoneNumber,
            cancellationToken);

        var (owners, totalQueryCounts) = ownersAndTotalQueryCounts;

        var ownersDto = _mapper.Map<IEnumerable<OwnerDto>>(owners);

        return new PagedList<OwnerDto>(ownersDto, request.Page, request.PageSize, totalQueryCounts);
    }
}

