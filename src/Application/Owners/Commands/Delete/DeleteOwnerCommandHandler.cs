using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Owners.Commands.Delete;
public class DeleteOwnerCommandHandler : ICommandHandler<DeleteOwnerCommand, Unit>
{
    private readonly IOwnersRepository _ownersRepo;
    private readonly ILogger<DeleteOwnerCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOwnerCommandHandler
        (
            IOwnersRepository ownersRepo,
            ILogger<DeleteOwnerCommandHandler> logger,
            IUserContext userContext,
            IUnitOfWork unitOfWork
        )
    {
        _ownersRepo = ownersRepo;
        _logger = logger;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Unit>> Handle(DeleteOwnerCommand request, CancellationToken cancellationToken)
    {
        if (_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return Result<Unit>.Failure(OwnerErrors.ForbidToDeleteOwner, HttpStatusCode.Forbidden);
        }
        if(!await _ownersRepo.OwnerExistsByOwnerIdAsync(request.OwnerId, cancellationToken))
        {
            return Result<Unit>.Failure(OwnerErrors.OwnerNotFound, HttpStatusCode.NotFound);
        }

        await _ownersRepo.DeleteOwnerByOwnerIdAsync(request.OwnerId, cancellationToken);
        
        await _unitOfWork.CommitAsync(cancellationToken);
        _logger.LogInformation("The admin with '{userId}' ID has delete the owner with " +
            "{uId}", _userContext.GetUserId(), request.OwnerId);
        
        return Result<Unit>.Success(HttpStatusCode.NoContent);
    }
}
