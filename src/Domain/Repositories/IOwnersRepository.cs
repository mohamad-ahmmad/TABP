using Domain.Entities;

namespace Domain.Repositories;
public interface IOwnersRepository
{
    Task AddOwnerAsync(Owner owner);
}

