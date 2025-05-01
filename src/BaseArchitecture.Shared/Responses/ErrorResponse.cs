namespace BaseArchitecture.Shared.Responses;

public sealed record ErrorResponse(string Message, List<string> Errors)
{
    public bool Success => false;
}
