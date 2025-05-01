using BaseArchitecture.Shared.MarkerInterfaces;

namespace BaseArchitecture.Shared.Responses;

public sealed record PagedDbResult<T>(
    List<T> Data,
    int TotalCount,
    int PageNumber,
    int PageSize = 10
)
    where T : IDomainEntity
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasPreviousPage => PageSize > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public bool Success => true;
}
