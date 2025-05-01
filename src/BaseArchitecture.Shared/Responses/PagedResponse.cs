namespace BaseArchitecture.Shared.Responses;

public sealed record PagedResponse<T>(
    List<T> Data,
    int TotalCount,
    int PageNumber,
    int PageSize = 10
)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasPreviousPage => PageSize > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public bool Success => true;
}
