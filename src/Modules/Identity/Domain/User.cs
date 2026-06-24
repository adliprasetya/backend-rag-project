using SharedKernel.Domain;

namespace Identity.Domain;

public class User : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }

    private User() { }

    public static User Create(string name, string email, string passwordHash)
    {
        return new User
        {
            Name = name,
            Email = email.ToLowerInvariant(),
            PasswordHash = passwordHash,
        };
    }
}
