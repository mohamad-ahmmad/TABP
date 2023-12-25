using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Users;

public class UsersRepository : IUsersRepository
{
    private readonly TABPDbContext _dbContext;

    public UsersRepository(TABPDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> AddUserAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(user, cancellationToken);
        return user;
    }
    public async Task<bool> IsUsernameExistByAsync(string username, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.Where(u => u.Username == username).AnyAsync(cancellationToken);
    }
    public async Task<bool> IsEmailExistByAsync(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.Where(u => u.Email == email).AnyAsync(cancellationToken);
    }

    public async Task<User?> GetUserByCredentials(string username, string password, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Where(u => u.Username == username &&  u.Password == password)
            .FirstOrDefaultAsync(cancellationToken);
    }
}

