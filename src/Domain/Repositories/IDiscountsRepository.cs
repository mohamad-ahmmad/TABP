using Domain.Entities;

namespace Domain.Repositories;
public interface IDiscountsRepository
{
    Task AddDiscountAsync(Discount discount, CancellationToken cancellationToken);
    Task<double> GetDiscountForRoomByRoomIdAsync(Guid roomId,
        CancellationToken cancellationToken = default);
}

