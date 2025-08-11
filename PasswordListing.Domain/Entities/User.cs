using System;

namespace PasswordListing.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public byte IsActive { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string FullName 
        => $"{FirstName} {LastName}";
    // TODO: Add ValueObjects if needed later
}
