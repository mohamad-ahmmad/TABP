using Domain.Shared;
using Square.Apis;
using Square.Exceptions;
using Square.Models;

namespace Infrastructure.Services.Payments;
public class SquarePaymentService
{
    private readonly IPaymentsApi _paymentsApi;

    public SquarePaymentService(IPaymentsApi paymentsApi)
    {
        _paymentsApi = paymentsApi;
    }

    public async Task<Result<Empty>> PayAsync(string cardDetailsToken,
        string idempotencyKey,
        double amount,
        string currency)
    {
        long amountLong = (long)(amount * 100);
        var createPaymentReq = new CreatePaymentRequest.Builder
            (
                cardDetailsToken,
                idempotencyKey
            )
            .AmountMoney
            (
               new Money.Builder()
                .Amount(amountLong)
                .Currency(currency)
                .Build()
            )
            .Autocomplete(true)
            .Build();
        try
        {
            CreatePaymentResponse result = await _paymentsApi.CreatePaymentAsync(createPaymentReq);
            var errors = result.Errors
                .Select(e => new Domain.Shared.Error(e.Category, e.Detail))
                .ToList();
            if (errors.Count != 0)
            {
                return Result<Empty>.Failures(errors);
            }
            
            return Result<Empty>.Success(Empty.Value)!;
        }
        catch (ApiException e)
        {
            throw;
        }
    }
}
