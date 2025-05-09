using BaseArchitecture.Shared.MarkerInterfaces;
using BaseArchitecture.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace BaseArchitecture.Infrastructure.Data.Extensions;

public static class ToPagedResult
{
    public static async Task<PagedDbResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    )
        where T : IDomainEntity
    {
        int count = await query.CountAsync(cancellationToken);

        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedDbResult<T>(data, count, page, pageSize);
    }
}
