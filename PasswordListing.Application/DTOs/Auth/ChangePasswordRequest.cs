using System;
using System.ComponentModel.DataAnnotations;

namespace PasswordListing.Application.DTOs.Auth;

public class ChangePasswordRequest
{
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string OldPassword { get; set; } = string.Empty;
    [Required]
    public string NewPassword { get; set; } = string.Empty;
}
