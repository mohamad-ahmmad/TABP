namespace Application.Abstractions.Data
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync(CancellationToken cancellationToken);
    }
}
