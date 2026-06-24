namespace SharedKernel.Domain;

public class TokenResult
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}

public interface IJwtService
{
    TokenResult GenerateToken(Guid userId, string email);
    Guid? ValidateToken(string token);
}
