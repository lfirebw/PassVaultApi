using System;

namespace PasswordListing.Infrastructure.Persistence;

public class SeedManager(AppDbContext appDbContext)
{
    private readonly AppDbContext _context = appDbContext;
    public async Task SeedDataAsync()
    {
        await new UserSeeder(_context).SeedAsync();
        await new ItemSeeder(_context).SeedAsync();
    }
}
