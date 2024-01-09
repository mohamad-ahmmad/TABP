using Domain.Entities;

namespace Domain.Repositories;
public interface IOwnersRepository
{
    Task AddOwnerAsync(Owner owner);
    Task<bool> OwnerExistsByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken);
    Task DeleteOwnerByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken);
    Task<(IEnumerable<Owner>, int)> GetOwnersAsync(int page,
        int pageSize,
        string? searchTerm,
        string? phoneNumber,
        CancellationToken cancellationToken);    
}

