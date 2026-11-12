using Domain;
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
}