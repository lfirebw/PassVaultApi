using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PasswordListing.Domain.Entities;
using PasswordListing.Domain.Security;

namespace PasswordListing.Infrastructure.Security;

public class JwtGenerator : IJwtGenerator
{
    private readonly IConfiguration _config;
    public JwtGenerator(IConfiguration configuration)
    {
        _config = configuration;
    }
    public string GenerateToken(User user)
    {
        Jwt JwtEntity = _config.GetSection("Jwt").Get<Jwt>() ?? throw new ApplicationException("Jwt not found");
        if(string.IsNullOrEmpty(JwtEntity.Key))
            throw new ApplicationException("Key is null");
        byte[] key = Encoding.UTF8.GetBytes(JwtEntity.Key);
        var signingKey = new SymmetricSecurityKey(key);
        var signingCredentials = new SigningCredentials(signingKey,SecurityAlgorithms.HmacSha256Signature);
        var tokenHandler = new JwtSecurityTokenHandler();
        int minuteRemain = 60;
        Claim[] claims = [
            new (ClaimTypes.Name, user.FullName),
            new (ClaimTypes.Email, user.Email),
            new (ClaimTypes.NameIdentifier, "PLAPIJWT"),
            new (JwtRegisteredClaimNames.Aud, JwtEntity.Audience),
            new (JwtRegisteredClaimNames.Iss, JwtEntity.Issuer)
        ];
        SecurityTokenDescriptor tokenDesc = new(){
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(minuteRemain),
            SigningCredentials = signingCredentials
        };
        SecurityToken token = tokenHandler.CreateToken(tokenDesc);

        return tokenHandler.WriteToken(token);
    }
    public string GenerateRefreshToken()
    {
        var rndNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(rndNumber);
        return Convert.ToBase64String(rndNumber); 
    }
}
