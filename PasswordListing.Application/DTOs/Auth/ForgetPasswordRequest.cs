using System;

namespace PasswordListing.Application.DTOs.Auth;

public class ForgetPasswordRequest
{
    public string Email { get; set; } = string.Empty;
    public string NewPass { get; set; } = string.Empty;
}
