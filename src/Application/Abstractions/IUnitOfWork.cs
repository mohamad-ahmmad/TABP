using System.Data;

namespace Application.Abstractions
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync(CancellationToken cancellationToken);
        Task BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken);
        Task CommitTransationAsync(CancellationToken cancellationToken);
        Task RollBackTransactionAsync(CancellationToken cancellationToken);
    }
}
