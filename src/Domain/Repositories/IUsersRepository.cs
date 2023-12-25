using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUsersRepository
    {
        Task<User> AddUserAsync(User user, CancellationToken cancellationToken);
        Task<bool> IsUsernameExistByAsync(string username, CancellationToken cancellationToken);
        Task<bool> IsEmailExistByAsync(string email, CancellationToken cancellationToken);
        Task<User?> GetUserByCredentials(string username, string password, CancellationToken cancellationToken);
    }
}
