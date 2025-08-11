using System;
using PasswordListing.Domain.Entities;

namespace PasswordListing.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
}
