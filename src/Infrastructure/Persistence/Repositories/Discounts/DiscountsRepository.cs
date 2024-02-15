using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Persistence.Repositories.Discounts;
public class DiscountsRepository : IDiscountsRepository
{
    private readonly TABPDbContext _dbContext;

    public DiscountsRepository(TABPDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddDiscountAsync(Discount discount, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(discount, cancellationToken);
    }
}
