using System;

namespace PasswordListing.Application.DTOs.Item;

public class CreateItemRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
