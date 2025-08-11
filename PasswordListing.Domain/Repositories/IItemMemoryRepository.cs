using System;
using PasswordListing.Domain.Entities;

namespace PasswordListing.Domain.Repositories;

public interface IItemMemoryRepository
{
    Task<Item?> GetByIdAsync(Guid id);
    Task<IEnumerable<Item>> GetAllAsync();
    Task<bool> CreateAsync(Item item);
    Task<bool> UpdateAsync(Item item);
    Task<bool> DeleteAsync(Item item);
}
