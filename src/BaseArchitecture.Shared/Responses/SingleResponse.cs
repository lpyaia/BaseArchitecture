namespace BaseArchitecture.Shared.Responses;

public sealed record SingleResponse<T>(T Data)
{
    public bool Success => true;
}
