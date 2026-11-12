using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain;
using Domain.Contexts.Account.Entities.Users;
using Domain.Contexts.Account.ValueObjects;
using Domain.Contexts.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecWithJWTeBearer.DataTransferObjects;

namespace SecWithJWTeBearer.Services;

public class AuthService(DatabaseContext databaseContext)
{
    public string GenerateToken(User user)
    {
        var handler = new JwtSecurityTokenHandler(); 

        var jwtTokenBytes = Encoding.ASCII.GetBytes(Configuration.Secrets.JwtToken);
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
        var user = await databaseContext.Users.FirstOrDefaultAsync(u => u.Email.Address == loginRequest.Email);

        if (user == null)
            return false;
        
        return Password.Verify(user.Password.Hash,  loginRequest.Password);
    }
}