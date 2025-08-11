using System;
using PasswordListing.Domain.Entities;
using PasswordListing.Domain.Repositories;

namespace PasswordListing.Infrastructure.Repositories;

public class InMemoryItemRepository : IItemMemoryRepository
{
    private readonly List<Item> _items = [];
    public async Task<bool> CreateAsync(Item item)
    {
        await Task.Delay(2);
        _items.Add(item);
        return true;
    }

    public async Task<bool> DeleteAsync(Item item)
    {
        await Task.Delay(200);
        if(!_items.Exists(el=> el.Id == item.Id))
            return false;
        _items.Remove(item);
        return true;
    }

    public Task<IEnumerable<Item>> GetAllAsync() =>
        Task.FromResult(_items.AsEnumerable());

    public Task<Item?> GetByIdAsync(Guid id) => 
        Task.FromResult(_items.FirstOrDefault(e=> e.Id == id));

    public async Task<bool> UpdateAsync(Item item)
    {
        await Task.Delay(200);
        var existItem = _items.FirstOrDefault(u => u.Id == item.Id);
        if (existItem == null)
            throw new ApplicationException("User not found");
        
        existItem.Name = item.Name;
        existItem.Description = item.Description;
        existItem.Value = item.Value;
        existItem.IsActive = item.IsActive;

        return true;
    }
}
