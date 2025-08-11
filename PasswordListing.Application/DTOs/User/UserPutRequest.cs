using System;

namespace PasswordListing.Application.DTOs.User;

public class UserPutRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public byte IsActive { get; set; } = 0;
}
