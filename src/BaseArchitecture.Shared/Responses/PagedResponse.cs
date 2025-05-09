using BaseArchitecture.Shared.MarkerInterfaces;

namespace BaseArchitecture.Shared.Responses;

public sealed record PagedResponse<T>
    where T : IDto
{
    public List<T> Data { get; init; }

    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    public int TotalCount { get; init; }

    public int TotalPages { get; init; }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public bool Success => true;

    public PagedResponse(List<T> data, int totalCount, int pageNumber, int pageSize = 10)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}
