using Application.Abstractions;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Discounts;
public class DiscountsRepository : IDiscountsRepository
{
    private readonly TABPDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public DiscountsRepository(TABPDbContext dbContext,
        IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }
    public async Task AddDiscountAsync(Discount discount, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(discount, cancellationToken);
    }

    public async Task<double> GetDiscountForRoomByRoomIdAsync(Guid roomId, CancellationToken cancellationToken)
    {
        return await _dbContext.Discounts.Where(d =>
        d.FromDate.CompareTo(_dateTimeProvider.GetUtcNow()) <= 0 &&
        d.ToDate.CompareTo(_dateTimeProvider.GetUtcNow()) >= 0)
            .Select(d => d.DiscountPercentage)
            .FirstOrDefaultAsync();
    }
}
