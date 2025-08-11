using System;
using Microsoft.EntityFrameworkCore;
using PasswordListing.Domain.Entities;

namespace PasswordListing.Infrastructure.Persistence;

public class UserSeeder(AppDbContext appDbContext)
{
    private readonly AppDbContext _context = appDbContext;
    public async Task SeedAsync()
    {
        if (_context.Users.Any())
        {
            Console.WriteLine("Users existed. Skip the seed process");
            return;
        }

        var users = new List<User>
        {
            new() {
                Id = Guid.Parse("de2fb02d-94a1-4623-8539-a512535a951b"),
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@example.com",
                PasswordHash = HashPassword("123456"),
                IsActive = 1,
                AvatarUrl = "https://placehold.co/100x100",
                CreatedAt = DateTime.UtcNow
            },
            new() {
                Id = Guid.Parse("92777600-429e-4b9a-be07-b31eac216211"),
                Email = "john@example.com",
                FirstName = "John",
                LastName = "Doe",
                PasswordHash = HashPassword("password"),
                IsActive = 1,
                AvatarUrl = "https://placehold.co/100x100",
                CreatedAt = DateTime.UtcNow
            }
        };

        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();
        Console.WriteLine("Users seed successfully.");
    }
    private string HashPassword(string password)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        return Convert.ToBase64String(sha.ComputeHash(bytes));
    }
}
