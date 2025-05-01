using BaseArchitecture.Shared.MarkerInterfaces;
using BaseArchitecture.Shared.Responses;

namespace BaseArchitecture.Infrastructure.Data.Extensions;

public static class PagedDbResultExtensions
{
    public static PagedResponse<TDestination> Convert<TSource, TDestination>(
        this PagedDbResult<TSource> source,
        Func<TSource, TDestination> converter
    )
        where TSource : IDomainEntity
        where TDestination : IDto
    {
        var convertedData = source.Data.Select(converter).ToList();

        return new PagedResponse<TDestination>(
            convertedData,
            source.TotalCount,
            source.PageNumber,
            source.PageSize
        );
    }
}
