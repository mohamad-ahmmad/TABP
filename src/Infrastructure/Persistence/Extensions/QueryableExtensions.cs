using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Extensions;

public static class QueryableExtensions
{
    public static async Task<List<T>> ToPagedListAsync<T>(this IQueryable<T> query, int page, int pageSize, CancellationToken cancellationToken)
    {
        if (page < 1)
            throw new ArgumentException("Page index must be greater than or equal to 1.", nameof(page));

        if (pageSize < 1)
            throw new ArgumentException("Page size must be greater than or equal to 1.", nameof(pageSize));

        int skip = (page - 1) * pageSize;

        return await query.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);
    }
}

