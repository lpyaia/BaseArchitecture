namespace BaseArchitecture.Shared.Responses;

public static class Response
{
    public static SingleResponse<T> CreateSingleResponse<T>(T data) => new SingleResponse<T>(data);

    public static PagedResponse<T> CreatePagedResponse<T>(
        List<T> data,
        int totalCount,
        int pageNumber
    ) => new PagedResponse<T>(data, totalCount, pageNumber);

    public static ErrorResponse CreateErrorResponse(string message, List<string> errors) =>
        new ErrorResponse(message, errors);
}
