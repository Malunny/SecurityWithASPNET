using System.Text;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecWithJWTeBearer.Exceptions;

namespace SecWithJWTeBearer.Extensions;

public static class BuilderExtensions
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.Secrets.JwtToken = 
            builder.Configuration["Secrets:JwtToken"] ?? throw new MissingEnvironmentVariableException("Secrets:JwtToken");
        Configuration.Secrets.PasswordSaltKey = 
            builder.Configuration["Secrets:PasswordSaltKey"] ?? throw new MissingEnvironmentVariableException("Secrets:PasswordSaltKey");
        Configuration.ConnectionStrings.DefaultConnection =
            builder.Configuration["ConnectionStrings:DefaultConnection"] ?? throw new MissingEnvironmentVariableException("ConnectionStrings:DefaultConnection");
    }

    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(Configuration.ConnectionStrings.DefaultConnection));
    }

    public static void AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearerOptions =>
        {
            var jwtTokenBytes = Encoding.ASCII.GetBytes(Configuration.Secrets.JwtToken);
            bearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(jwtTokenBytes),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
    }

    public static void AddAuthorizationPolicies(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Logged",  policy => policy.RequireAuthenticatedUser());
            options.AddPolicy("Pro",  policy => policy.RequireRole("Pro", "Admin"));
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
        });
    }
}