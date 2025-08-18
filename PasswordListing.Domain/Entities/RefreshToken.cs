namespace PasswordListing.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public byte IsActive { get; set; } = 0;
    public string CreatedByIp { get; set; } = string.Empty;
    public DateTime Created { get; set; }

    public User? User { get; set; }

    public bool IsExpired => DateTime.UtcNow >= Expires;
}