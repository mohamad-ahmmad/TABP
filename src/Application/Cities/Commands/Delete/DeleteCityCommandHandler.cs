using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Enums;
using Domain.Errors;
using Domain.Repositories;
using Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Cities.Commands.Delete;
public class DeleteCityCommandHandler : ICommandHandler<DeleteCityCommand, Unit>
{
    private readonly ICitiesRepository _citiesRepo;
    private readonly ILogger<DeleteCityCommand> _logger;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCityCommandHandler(
        ICitiesRepository citiesRepo,
        ILogger<DeleteCityCommand> logger,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _citiesRepo = citiesRepo;
        _logger = logger;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Unit>> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        if (_userContext.GetUserLevel() != UserLevels.Admin)
        {
            return Result<Unit>.Failure(CityErrors.UnauthorizedToDeleteCity, HttpStatusCode.Forbidden);
        }

        if(!await _citiesRepo.DoesCityExistsByIdAsync(request.CityId, cancellationToken))
        {
            return Result<Unit>.Failure(
                CityErrors.CreateCityNotFoundError("Delete city", request.CityId),
                HttpStatusCode.NotFound);
        }
        
        _citiesRepo.DeleteCityById(request.CityId);
        
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("City with '{cityId}' has been deleted by user with '{userId}' ID"
            ,request.CityId, _userContext.GetUserId());

        return Result<Unit>.Success(Unit.Value);
    }

}

