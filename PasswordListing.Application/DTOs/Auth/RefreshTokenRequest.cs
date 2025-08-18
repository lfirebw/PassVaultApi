using System;

namespace PasswordListing.Application.DTOs.Auth;

public class RefreshTokenRequest
{
    public string IpAddress { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
