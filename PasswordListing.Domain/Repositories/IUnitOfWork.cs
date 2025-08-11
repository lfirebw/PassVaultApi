using System;

namespace PasswordListing.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IItemRepository Items { get; }
    IGenericRepository<T> Repository<T>() where T : class;
    Task<int> SaveChangesAsync();
}
