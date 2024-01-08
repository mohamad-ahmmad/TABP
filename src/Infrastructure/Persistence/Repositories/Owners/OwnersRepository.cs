using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Repositories.Owners;
public class OwnersRepository : IOwnersRepository
{
    private readonly TABPDbContext _dbContext;
    private readonly ILogger<OwnersRepository> _logger;

    public OwnersRepository(TABPDbContext dbContext, ILogger<OwnersRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task AddOwnerAsync(Owner owner)
    {
        await _dbContext.AddAsync(owner);
        _logger.LogInformation("User with '{userId}' has been tracked as '{entityState}'"
            , owner.Id, "EntityState.Added");
    }
}

