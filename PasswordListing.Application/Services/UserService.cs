using System;
using PasswordListing.Application.DTOs.User;
using PasswordListing.Application.Interfaces;
using PasswordListing.Domain.Entities;
using PasswordListing.Domain.Repositories;
using System.Security.Claims;

namespace PasswordListing.Application.Services;

public class UserService : IUserService
{
    private readonly IUserMemoryRepository _userRepository;

    public UserService(IUserMemoryRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        List<UserResponse> result = [];
        var data = await _userRepository.GetAllAsync();
        if(data==null)
            return result;
        var prepare = data.Select(user=>new UserResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl
            });
        result.AddRange(prepare);
        return result;
    }
    public async Task<UserResponse?> GetByIdAsync(string id)
    {
        Guid guidParse = Guid.Parse(id);
        return await GetByIdAsync(guidParse);
    }
    public async Task<UserResponse?> GetByIdAsync(Guid id)
    {
        var findUser = await _userRepository.GetByIdAsync(id);
        if(findUser == null)
            return null;
        return new UserResponse
        {
            FirstName = findUser.FirstName,
            LastName = findUser.LastName,
            FullName = $"{findUser.FirstName} {findUser.LastName}",
            Email = findUser.Email,
            AvatarUrl = findUser.AvatarUrl
        };
    }
    public async Task<UserResponse?> GetCurrentUser(ClaimsPrincipal userClaims)
    {
        string email = userClaims.FindFirst(ClaimTypes.Email)?.Value ?? throw new UnauthorizedAccessException("User not found");
        var findUser = await _userRepository.GetByEmailAsync(email);
        if(findUser == null)
            return null;
        return new UserResponse
        {
            FirstName = findUser.FirstName,
            LastName = findUser.LastName,
            FullName = $"{findUser.FirstName} {findUser.LastName}",
            Email = findUser.Email,
            AvatarUrl = findUser.AvatarUrl
        };
    }
    public async Task<bool> Update(string guid, UserPutRequest request)
    {
        Guid guidParse = Guid.Parse(guid);
        var findUser = await _userRepository.GetByIdAsync(guidParse);
        if(findUser == null)
            return false;
        bool result = await _userRepository.Update(findUser);
        return result;
    }
    public async Task<bool> UpdateStatus(string id, byte status)
    {
        Guid guidParse = Guid.Parse(id);
        var findUser = await _userRepository.GetByIdAsync(guidParse);
        if(findUser == null)
            return false;
        findUser.IsActive = status;
        bool result = await _userRepository.Update(findUser);
        return result;
    }
    public async Task<bool> Delete(string id)
    {
        Guid guidParse = Guid.Parse(id);
        var findUser = await _userRepository.GetByIdAsync(guidParse);
        if(findUser == null)
            return false;
        bool result = await _userRepository.Delete(findUser);
        return result;
    }
}
