using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
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
        owner.FirstName = owner.FirstName.ToLower();
        owner.LastName = owner.LastName.ToLower();
        await _dbContext.AddAsync(owner);
        _logger.LogInformation("User with '{userId}' has been tracked as '{entityState}'"
            , owner.Id, "EntityState.Added");
    }
    
    public async Task DeleteOwnerByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken)
    {
       var user = await _dbContext.Owners.Where(o => o.Id == ownerId).FirstAsync(cancellationToken);
       user.IsDeleted = true;
    }

    public async Task<Owner?> GetOwnerByIdAsync(Guid ownerId, CancellationToken cancellationToken)
    {
        return await _dbContext.Owners.Where(o => o.Id == ownerId && o.IsDeleted == false).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Owner>, int)> GetOwnersAsync(int page,
        int pageSize,
        string? searchTerm,
        string? phoneNumber,
        CancellationToken cancellationToken)
    {
        IQueryable<Owner> query = _dbContext.Owners.Where(o => o.IsDeleted == false);

        if(searchTerm != null)
        {
            query = query.Where(o => o.FirstName.Contains(searchTerm) || o.LastName.Contains(searchTerm));
        }

        if(phoneNumber != null)
        {
            query = query.Where(o => o.PhoneNumber.Contains(phoneNumber));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var pageList = await query.OrderBy(o => o.FirstName).ToPagedListAsync(page, pageSize, cancellationToken);

        return (pageList, totalCount);
    }

    public async Task<bool> OwnerExistsByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken)
    {
       return await _dbContext.Owners.AnyAsync(o => o.Id == ownerId && o.IsDeleted == false, cancellationToken);
    }
}
