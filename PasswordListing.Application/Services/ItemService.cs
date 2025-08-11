using System;
using PasswordListing.Application.DTOs.Item;
using PasswordListing.Application.Interfaces;
using PasswordListing.Domain.Entities;
using PasswordListing.Domain.Repositories;

namespace PasswordListing.Application.Services;

public class ItemService : IItemService
{
    private readonly IItemMemoryRepository _itemRepository;

    public ItemService(IItemMemoryRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }
    public async Task<IEnumerable<ItemResponse>> GetAllAsync()
    {
        var items = await _itemRepository.GetAllAsync();
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

        var item = await _itemRepository.GetByIdAsync(guid);
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

        return await _itemRepository.CreateAsync(newItem);
    }
    public async Task<bool> UpdateAsync(string id, UpdateItemRequest request)
    {
        if (!Guid.TryParse(id, out var guid))
            return false;

        var existingItem = await _itemRepository.GetByIdAsync(guid);
        if (existingItem == null)
            return false;

        existingItem.Name = request.Name;
        existingItem.Description = request.Description;
        existingItem.Value = request.Value;

        return await _itemRepository.UpdateAsync(existingItem);
    }
    public async Task<bool> DeleteAsync(string id)
    {
        if (!Guid.TryParse(id, out var guid))
            return false;

        var existingItem = await _itemRepository.GetByIdAsync(guid);
        if (existingItem == null)
            return false;

        return await _itemRepository.DeleteAsync(existingItem);
    }
}
