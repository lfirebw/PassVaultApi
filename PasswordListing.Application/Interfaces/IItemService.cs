using System;
using PasswordListing.Application.DTOs.Item;

namespace PasswordListing.Application.Interfaces;

public interface IItemService
{
    Task<IEnumerable<ItemResponse>> GetAllAsync();
    Task<ItemResponse?> GetByIdAsync(string id);
    Task<bool> CreateAsync(CreateItemRequest request);
    Task<bool> UpdateAsync(string id, UpdateItemRequest request);
    Task<bool> DeleteAsync(string id);
}
