using System;
using PasswordListing.Application.DTOs.Item;
using PasswordListing.Application.Interfaces;
using PasswordListing.Domain.Entities;
using PasswordListing.Domain.Repositories;

namespace PasswordListing.Application.Services;

public class ItemService : IItemService
{
    private readonly IUnitOfWork _persistence;

    public ItemService(IUnitOfWork persistence)
    {
        _persistence = persistence;
    }
    public async Task<IEnumerable<ItemResponse>> GetAllAsync()
    {
        var items = await _persistence.Items.GetAllAsync();
        return items.Select(i => new ItemResponse
        {
            Name = i.Name,
            Description = i.Description,
            Value = i.Value
        });
    }
    public async Task<ItemResponse?> GetByIdAsync(string id)
    {
        if (!Guid.TryParse(id, out var guid))
            return null;

        var item = await _persistence.Items.GetByIdAsync(guid);
        if (item == null) return null;

        return new ItemResponse
        {
            Name = item.Name,
            Description = item.Description,
            Value = item.Value
        };
    }
    public async Task<bool> CreateAsync(CreateItemRequest request)
    {
        string valueHash = request.Value;
        var newItem = new Item
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Value = valueHash,
            IsActive = 1,
            CreatedAt = DateTime.UtcNow
        };

        await _persistence.Items.AddAsync(newItem);
        await _persistence.SaveChangesAsync();
        return true;
    }
    public async Task<bool> UpdateAsync(string id, UpdateItemRequest request)
    {
        if (!Guid.TryParse(id, out var guid))
            return false;

        var existingItem = await _persistence.Items.GetByIdAsync(guid);
        if (existingItem == null)
            return false;

        existingItem.Name = request.Name;
        existingItem.Description = request.Description;
        existingItem.Value = request.Value;

        await _persistence.Items.UpdateAsync(existingItem);
        await _persistence.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DeleteAsync(string id)
    {
        if (!Guid.TryParse(id, out var guid))
            return false;

        var existingItem = await _persistence.Items.GetByIdAsync(guid);
        if (existingItem == null)
            return false;

        await _persistence.Items.DeleteAsync(existingItem);
        await _persistence.SaveChangesAsync();
        return true;
    }
}
