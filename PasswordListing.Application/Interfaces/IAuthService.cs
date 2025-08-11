using System;
using PasswordListing.Application.DTOs.Auth;

namespace PasswordListing.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    Task<bool> ForgetPasswordAsync(string email, string newPassword = "123456");
}
