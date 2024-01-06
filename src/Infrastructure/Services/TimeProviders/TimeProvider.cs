using Application.Abstractions;

namespace Infrastructure.Services.TimeProviders;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetUtcNow()
    {
        return DateTime.UtcNow;
    }
}

