using System;
using PasswordListing.Domain.Entities;

namespace PasswordListing.Domain.Repositories;

public interface IUserRepository: IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}
