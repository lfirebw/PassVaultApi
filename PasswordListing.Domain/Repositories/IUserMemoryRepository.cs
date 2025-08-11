using System;
using PasswordListing.Domain.Entities;

namespace PasswordListing.Domain.Repositories;

public interface IUserMemoryRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User user);
    Task SaveChangesAsync();
    Task<bool> Update(User user);
    Task<bool> Delete(User user);
}
