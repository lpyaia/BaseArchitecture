namespace BasicArchitecture.Api.Auth.DTOs;

public record AuthResponseDto(
    bool Success,
    string? Token = null,
    string? RefeshToken = null,
    DateTime? Expiration = null,
    IEnumerable<string>? Errors = null
);
