namespace Application.Abstractions
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync(CancellationToken cancellationToken);
    }
}
