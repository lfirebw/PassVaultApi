using System;
using Microsoft.EntityFrameworkCore;
using PasswordListing.Domain.Entities;
using PasswordListing.Domain.Repositories;
using PasswordListing.Infrastructure.Persistence;

namespace PasswordListing.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly AppDbContext _appContext;
    public UserRepository(AppDbContext context) : base(context){
        _appContext = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _appContext.Users.Where(u=>u.Email == email).FirstOrDefaultAsync();
    }
}
