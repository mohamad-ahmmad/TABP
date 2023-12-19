using Application.Abstractions;

namespace Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TABPDbContext _dbContext;

        public UnitOfWork(TABPDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CommitAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
