using Domain.Shared;

namespace Application.Abstractions;
public interface IPaymentService
{
    Task<Result<Empty>> PayAsync(string cardDetailsToken,
        string idempotencyKey,
        double amount,
        string currency);
}
