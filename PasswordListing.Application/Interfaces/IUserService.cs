using System;
using System.Security.Claims;
using PasswordListing.Application.DTOs.User;

namespace PasswordListing.Application.Interfaces;

public interface IUserService
{
    Task<UserResponse?> GetByIdAsync(string id);
    Task<UserResponse?> GetByIdAsync(Guid id);
    Task<IEnumerable<UserResponse>> GetAllAsync();
    Task<UserResponse?> GetCurrentUser(ClaimsPrincipal userClaims);
    Task<bool> Update(string guid,UserPutRequest request);
    Task<bool> UpdateStatus(string id, byte status);
    Task<bool> Delete(string id);
}
