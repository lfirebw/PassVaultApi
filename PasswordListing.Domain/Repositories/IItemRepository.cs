using System;
using PasswordListing.Domain.Entities;

namespace PasswordListing.Domain.Repositories;

public interface IItemRepository : IGenericRepository<Item>
{
    void Dispose();
}
