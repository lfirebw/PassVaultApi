using System;
using PasswordListing.Domain.Entities;

namespace PasswordListing.Infrastructure.Persistence;

public class ItemSeeder(AppDbContext appDbContext)
{
    private readonly AppDbContext _context = appDbContext;
    public async Task SeedAsync()
    {
        if (_context.Items.Any())
        {
            Console.WriteLine("items existed. Skip the seed process");
            return;
        }

        var items = new List<Item>
        {
            new() { 
                Id = Guid.NewGuid(), 
                Name = "Item 1", 
                Description = "Description 1" ,
                IsActive = 1,
                CreatedAt = DateTime.UtcNow
            },
            new() { 
                Id = Guid.NewGuid(), 
                Name = "Item 2", 
                Description = "Description 2" ,
                IsActive = 1,
                CreatedAt = DateTime.UtcNow
            },
        };

        await _context.Items.AddRangeAsync(items);
        await _context.SaveChangesAsync();
        Console.WriteLine("Items seed successfully.");
    }
}
