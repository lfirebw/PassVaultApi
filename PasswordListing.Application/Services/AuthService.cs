using System;
using System.Security.Cryptography;
using System.Text;
using PasswordListing.Application.DTOs.Auth;
using PasswordListing.Application.Interfaces;
using PasswordListing.Domain.Repositories;
using PasswordListing.Domain.Security;

namespace PasswordListing.Application.Services;

public class AuthService : IAuthService
{
    private IUserMemoryRepository _users;
    private IJwtGenerator _jwtGenerator;
    public AuthService(IUserMemoryRepository userRepository, IJwtGenerator jwtGenerator)
    {
        _users = userRepository;
        _jwtGenerator = jwtGenerator;
    }
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _users.GetByEmailAsync(request.Email);
        if(user == null || !VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");
        return  new LoginResponse
        {
            Token = _jwtGenerator.GenerateToken(user),
            Email = user.Email,
            FullName = user.FullName
        };
    }
    public async Task<bool> ChangePasswordAsync(string userEmail, string currentPassword, string newPassword)
    {
        var user = await _users.GetByEmailAsync(userEmail);
        if (user == null)
            return false;
        if (!VerifyPassword(user.PasswordHash, currentPassword))
            return false;
        user.PasswordHash = HashPassword(newPassword);
        return await _users.Update(user);
    }
    public async Task<bool> ForgetPasswordAsync(string email, string newPassword = "123456")
    {
        var user = await _users.GetByEmailAsync(email);
        if (user == null)
            return false;
        user.PasswordHash = HashPassword(newPassword);
        return await _users.Update(user);
    }
    private bool VerifyPassword(string password, string storedHash)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = Convert.ToBase64String(SHA256.HashData(bytes));
        return hash == storedHash;
    }
    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        return Convert.ToBase64String(sha.ComputeHash(bytes));
    }
}
