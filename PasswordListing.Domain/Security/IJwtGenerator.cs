using System;
using PasswordListing.Domain.Entities;

namespace PasswordListing.Domain.Security;

public interface IJwtGenerator
{
    string GenerateToken(User user);
}
