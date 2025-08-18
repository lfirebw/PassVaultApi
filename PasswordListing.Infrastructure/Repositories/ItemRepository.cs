using System;
using PasswordListing.Domain.Entities;
using PasswordListing.Domain.Repositories;
using PasswordListing.Infrastructure.Persistence;

namespace PasswordListing.Infrastructure.Repositories;

public class ItemRepository : GenericRepository<Item>, IItemRepository
{
    public ItemRepository(AppDbContext context) : base(context) {}
    public void Dispose(){
        throw new ApplicationException();
    }
}
