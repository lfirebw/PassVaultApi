using System;

namespace PasswordListing.Domain.Entities;

public class Item
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public byte IsActive { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
