using Application.Abstractions;
using Application.Users.Queries.Login;
using Domain.Common;
using Infrastructure.Persistence.Repositories.Users;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly IUserContext _userContext;
    private readonly IDateTimeProvider _dateProvider;

    public AuditableEntityInterceptor(IUserContext userContext, IDateTimeProvider dateProvider)
    {
        _userContext = userContext;
        _dateProvider = dateProvider;
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

        foreach (var entry in dbContext.ChangeTracker.Entries<BaseSoftDeletableAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = _userContext.GetUserId();
                entry.Entity.Created = _dateProvider.GetUtcNow();
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || HasChangedOwnedEntities(entry))
            {
                entry.Entity.LastModifiedBy = _userContext.GetUserId();
                entry.Entity.LastModified = _dateProvider.GetUtcNow();
            }
        }
        
    }
    public static bool HasChangedOwnedEntities(EntityEntry entry) =>
      entry.References.Any(r =>
          r.TargetEntry != null &&
          r.TargetEntry.Metadata.IsOwned() &&
          (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));

}

