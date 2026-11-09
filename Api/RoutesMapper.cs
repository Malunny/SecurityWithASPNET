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
                .FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

            if (user == null)
            {
                return Results.Unauthorized();
            }

            return Results.Ok(authService.GenerateToken(user));
        });
    }

    public static void AddGets(this WebApplication app)
    {
        app.MapGet("/authenticated/admin", () => "Você é admin").RequireAuthorization("Admin");
    }
}