namespace BasicArchitecture.Api.Auth.DTOs;

public record RegisterRequestDto(
    string Email,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string LastName,
    string? PhoneNumber = null
);
