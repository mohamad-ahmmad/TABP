using Application.Abstractions;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly IUserContext _userContext;
    private readonly IDateTimeProvider _dateProvider;
    private readonly ILogger<AuditableEntityInterceptor> _logger;

    public AuditableEntityInterceptor(IUserContext userContext,
        IDateTimeProvider dateProvider,
        ILogger<AuditableEntityInterceptor> logger)
    {
        _userContext = userContext;
        _dateProvider = dateProvider;
        _logger = logger;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {

        UpdateAuditingInfo(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateAuditingInfo(DbContext? dbContext)
    {
        if (dbContext == null)
            return;

 
        foreach (var entry in dbContext.ChangeTracker
            .Entries<BaseSoftDeletableAuditableEntity>())
        {
            var id = entry.Entity.Id;
            var type = entry.Entity.GetType();
            var modifierId = _userContext.GetUserId();
            var modifiedDate = _dateProvider.GetUtcNow();

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = modifierId;
                entry.Entity.Created = modifiedDate;
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || HasChangedOwnedEntities(entry))
            {
                entry.Entity.LastModifiedBy = id;
                entry.Entity.LastModified = modifiedDate;
            }
            _logger.LogInformation("Entity with {type} type and '{entityId}' ID has been added/modified by admin " +
                "with '{adminId}' ID at {date}", type, id, modifierId, modifiedDate);
        }

    }
    public static bool HasChangedOwnedEntities(EntityEntry entry) =>
      entry.References.Any(r =>
          r.TargetEntry != null &&
          r.TargetEntry.Metadata.IsOwned() &&
          (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));

}

