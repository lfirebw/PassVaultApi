using System;
using PasswordListing.Domain.Entities;
using PasswordListing.Domain.Repositories;

namespace PasswordListing.Infrastructure.Repositories;

public class InMemoryUserRepository : IUserMemoryRepository
{
    private readonly List<User> _users;

    public InMemoryUserRepository()
    {
        // Mock de datos — en la vida real esto viene de DB
        _users = new List<User>
        {
            new User
            {
                Id = Guid.Parse("de2fb02d-94a1-4623-8539-a512535a951b"),
                Email = "admin@example.com",
                FirstName = "Admin",
                LastName = "User",
                PasswordHash = HashPassword("123456"), // Simulamos hash
                AvatarUrl = "https://placehold.co/100x100",
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = Guid.Parse("92777600-429e-4b9a-be07-b31eac216211"),
                Email = "john@example.com",
                FirstName = "John",
                LastName = "Doe",
                PasswordHash = HashPassword("password"),
                AvatarUrl = "https://placehold.co/100x100",
                CreatedAt = DateTime.UtcNow
            }
        };
    }

    public Task<IEnumerable<User>> GetAllAsync() =>
        Task.FromResult(_users.AsEnumerable());
    
    public Task<User?> GetByEmailAsync(string email) =>
        Task.FromResult(_users.FirstOrDefault(e=> e.Email == email));

    public Task<User?> GetByIdAsync(Guid id) =>
        Task.FromResult(_users.FirstOrDefault(el=> el.Id == id));
    public async Task AddAsync(User user)
    {
        await Task.Delay(200);
        _users.Add(user);
    }
    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
    public async Task<bool> Update(User user)
    {
        await Task.Delay(200);
        var existUser = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existUser == null)
            throw new ApplicationException("User not found");
        
        existUser.FirstName = user.FirstName;
        existUser.LastName = user.LastName;
        existUser.IsActive = user.IsActive;
        existUser.Email = user.Email;
        existUser.AvatarUrl = user.AvatarUrl;

        return true;
    }
    public async Task<bool> Delete(User user)
    {
        await Task.Delay(200);
        if(!_users.Exists(el=> el.Id == user.Id))
            return false;
        _users.Remove(user);
        return true;
    }
    private static string HashPassword(string password)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        return Convert.ToBase64String(sha.ComputeHash(bytes));
    }
}
