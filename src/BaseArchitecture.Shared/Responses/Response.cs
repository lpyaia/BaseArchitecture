using BaseArchitecture.Shared.MarkerInterfaces;

namespace BaseArchitecture.Shared.Responses;

public static class Response
{
    public static SingleResponse<T> CreateSingleResponse<T>(T data)
        where T : IDto => new SingleResponse<T>(data);

    public static PagedResponse<T> CreatePagedResponse<T>(
        List<T> data,
        int totalCount,
        int pageNumber
    )
        where T : IDto => new PagedResponse<T>(data, totalCount, pageNumber);

    public static ErrorResponse CreateErrorResponse(string message, List<string> errors) =>
        new ErrorResponse(message, errors);
}
