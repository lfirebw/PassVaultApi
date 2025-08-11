using System;
using PasswordListing.Domain.Repositories;
using PasswordListing.Infrastructure.Persistence;

namespace PasswordListing.Infrastructure.Repositories;

public class PersistenceContext : IUnitOfWork
{
    protected readonly AppDbContext _context;
    private IItemRepository? _itemRepository;
    private IUserRepository? _userRepository;
    // public IUserRepository Users { get; }
    // public IItemRepository Items { get; }
    public IItemRepository Items => _itemRepository ??= new ItemRepository(_context);
    public IUserRepository Users => _userRepository ??= new UserRepository(_context);


    public PersistenceContext(AppDbContext context)
    {
        _context = context;
    }
    public IGenericRepository<T> Repository<T>() where T : class =>
        new GenericRepository<T>(_context);
    public async Task<int> SaveChangesAsync() => 
        await _context.SaveChangesAsync();
    public void Dispose()
    {
        _context.Dispose();
    }
}
