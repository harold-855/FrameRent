namespace FrameRent.Application.Interfaces.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string email, string fullName, string role);
}
