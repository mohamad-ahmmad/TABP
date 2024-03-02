using Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TABPDbContext _dbContext;

        public UnitOfWork(TABPDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken)
        {
            await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task CommitTransationAsync(CancellationToken cancellationToken)
        {
            if(_dbContext.Database.CurrentTransaction == null)
            {
                return;
            }

            await _dbContext.Database.CurrentTransaction.CommitAsync(cancellationToken);
        }

        public async Task RollBackTransactionAsync(CancellationToken cancellationToken)
        {
            if(_dbContext.Database.CurrentTransaction == null)
            {
                return;
            }

            await _dbContext.Database.CurrentTransaction.RollbackAsync(cancellationToken);
        }
    }
}
