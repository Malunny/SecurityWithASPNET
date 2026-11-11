using Domain.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecWithJWTeBearer.DataTransferObjects;
using SecWithJWTeBearer.Services;

namespace SecWithJWTeBearer;

public static class RoutesMapper
{
    public static void AddPosts(this WebApplication app)
    {
        app.MapPost("/login", async ([FromServices] AuthService authService,
            [FromServices] DatabaseContext db, [FromBody] LoginRequest loginRequest) =>
        {
            var authenticated = await authService.AuthenticateUser(loginRequest);

            if (!authenticated)
            {
                return Results.Unauthorized();
            }

            var user = await db.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email.Address == loginRequest.Email);

            if (user == null)
            {
                return Results.Unauthorized();
            }

            return Results.Ok(authService.GenerateToken(user));
        });

        app.MapPost("/register", async ([FromServices] AuthService authService,
            [FromServices] DatabaseContext db,
            [FromBody] RegisterRequest registerRequest) =>
        {
            var alreadyExistsUser = await db.Users.AnyAsync(u => u.Email.Address == registerRequest.Email);
            
            if (alreadyExistsUser)
                return Results.Conflict();
            
            User user = new User(registerRequest.Name, registerRequest.Email, registerRequest.Password);

            Console.WriteLine($"User: {user.Email}: " + Environment.NewLine + 
            $"""
                {user.Email.Address}
                {user.Email.Verification.Code}
                {user.Email.Verification.ExpiresAt}
                {user.Email.Verification.VerifiedAt}
                {user.Email.Verification.IsActive}                                                                   
            """);
            
            var normalUserRole = await db.Roles.FirstOrDefaultAsync(role => role.Name == "Normal");
            
            if (normalUserRole != null)
                user.Roles.Add(normalUserRole);
            
            db.Users.Add(user);
            await db.SaveChangesAsync();
            
            return Results.Ok(authService.GenerateToken(user));
        });
    }

    public static void AddGets(this WebApplication app)
    {
        app.MapGet("/authenticated/admin", () => "Você é admin").RequireAuthorization("Admin");
        
        app.MapGet("/users", ([FromServices] DatabaseContext db) => 
            db.Users.Include(u => u.Roles)
                .Select(u => new UserGetResponse(u)));
    }
}