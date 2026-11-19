using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Domain;

public static class JwtExtensions
{
    public static readonly int ExpirationInHours = 3;
    public static string GenerateToken(IEnumerable<Claim> claims)
    {
        var tokenBytes = Encoding.UTF8.GetBytes(Configuration.Secrets.JwtToken);
        var key = new SymmetricSecurityKey(tokenBytes);
        
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(ExpirationInHours),
            SigningCredentials = signingCredentials
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);
    }
}