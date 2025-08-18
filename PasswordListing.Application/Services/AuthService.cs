using System;
using System.Security.Cryptography;
using System.Text;
using PasswordListing.Application.DTOs.Auth;
using PasswordListing.Application.Interfaces;
using PasswordListing.Domain.Entities;
using PasswordListing.Domain.Repositories;
using PasswordListing.Domain.Security;

namespace PasswordListing.Application.Services;

public class AuthService : IAuthService
{
    private IJwtGenerator _jwtGenerator;
    private IUnitOfWork _persistence;
    public AuthService(IJwtGenerator jwtGenerator, IUnitOfWork persistence)
    {
        _jwtGenerator = jwtGenerator;
        _persistence = persistence;
    }
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _persistence.Users.GetByEmailAsync(request.Email);
        if(user == null || !VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");
        var refreshToken = _jwtGenerator.GenerateRefreshToken();
        await UpdateAndSaveRefreshToken(user,refreshToken,request.IpAddress);
        return  new LoginResponse
        {
            Token = _jwtGenerator.GenerateToken(user),
            RefreshToken = refreshToken,
            Email = user.Email,
            FullName = user.FullName
        };
    }
    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        if(string.IsNullOrEmpty(request.RefreshToken))
            throw new UnauthorizedAccessException("Invalid credentials");
        //find refreshtoken and validate
        var refreshTokenRepo = _persistence.Repository<RefreshToken>();
        var findRefreshToken = refreshTokenRepo.Query().Where(t=>t.Token == request.RefreshToken && t.IsActive == 1).FirstOrDefault();
        if(findRefreshToken == null || findRefreshToken.IsExpired)
            throw new UnauthorizedAccessException("Session expired");
        //retrieve iduser and find user
        var user = await _persistence.Users.GetByIdAsync(findRefreshToken.UserId);
        if(user == null)
            throw new NullReferenceException("User not found");
        var refreshToken = _jwtGenerator.GenerateRefreshToken();

        await UpdateAndSaveRefreshToken(user,refreshToken,request.IpAddress);
        return new RefreshTokenResponse{
            AccessToken = _jwtGenerator.GenerateToken(user),
            RefreshToken = refreshToken
        };
    }
    public async Task<bool> ChangePasswordAsync(string userEmail, string currentPassword, string newPassword)
    {
        var user = await _persistence.Users.GetByEmailAsync(userEmail);
        if (user == null)
            return false;
        if (!VerifyPassword(user.PasswordHash, currentPassword))
            return false;
        user.PasswordHash = HashPassword(newPassword);
        await _persistence.Users.UpdateAsync(user);
        await _persistence.SaveChangesAsync();
        return true;
    }
    public async Task<bool> ForgetPasswordAsync(string email, string newPassword = "123456")
    {
        var user = await _persistence.Users.GetByEmailAsync(email);
        if (user == null)
            return false;
        user.PasswordHash = HashPassword(newPassword);
        await _persistence.Users.UpdateAsync(user);
        await _persistence.SaveChangesAsync();
        return true;
    }
    private async Task UpdateAndSaveRefreshToken(User user,string token, string ipAddress)
    {
        var refreshTokenRepo = _persistence.Repository<RefreshToken>();
        var listRefreshToken = refreshTokenRepo.Query().Where(t=>t.UserId == user.Id && t.IsActive == 1).ToList();
        foreach(var item in listRefreshToken)
            item.IsActive = 0;
        await refreshTokenRepo.UpdateRangeAsync(listRefreshToken);
        //save refreshtoken
        var newRefreshToken = new RefreshToken{
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = token,
            Expires = DateTime.UtcNow.AddDays(1),
            CreatedByIp = ipAddress,
            IsActive = 1,
            Created = DateTime.UtcNow
        };
        await refreshTokenRepo.AddAsync(newRefreshToken);
        await _persistence.SaveChangesAsync();
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
