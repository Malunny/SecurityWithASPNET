using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecWithJWTeBearer.DataTransferObjects;

namespace SecWithJWTeBearer.Services;

public class AuthService(DatabaseContext db)
{
    public string GenerateToken(User user)
    {
        var handler = new JwtSecurityTokenHandler(); // responsável por gerar o token.
        
        // gerando info do token

        var jwtTokenBytes = Encoding.ASCII.GetBytes(Configuration.JwtToken);
        var symmetricSecurityKey = new SymmetricSecurityKey(jwtTokenBytes);
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = signingCredentials,
            Expires = DateTime.UtcNow.AddHours(4),
            Subject = GenerateClaimsIdentity(user)
        };
        
        var token = handler.CreateToken(tokenDescriptor);
        
        return handler.WriteToken(token);
    }

    private ClaimsIdentity GenerateClaimsIdentity(User user)
    {
        var claimsIdentity = new ClaimsIdentity();
        
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Email));

        foreach (var role in user.Roles)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
        }
        
        return claimsIdentity;
    }

    public async Task<bool> AuthenticateUser(LoginRequest loginRequest)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
        
        if (user == null)
            return false;

        if (user.PasswordHash != loginRequest.Password)
            return false;
        
        return true;
    }
}