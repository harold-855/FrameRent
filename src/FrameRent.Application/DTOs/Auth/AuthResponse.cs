namespace FrameRent.Application.DTOs.Auth;

public record AuthResponse(
    string Token,
    string Email,
    string FullName,
    string Role,
    Guid UserId);
