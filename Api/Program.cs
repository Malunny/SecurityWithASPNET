using System.Text;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecWithJWTeBearer;
using SecWithJWTeBearer.DataTransferObjects;
using SecWithJWTeBearer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration["ConnectionString"];
var jwtToken = builder.Configuration["JwtToken"] ?? throw new Exception("Jwt Token not found");
var passwordSaltKey = builder.Configuration["PasswordSaltKey"] ?? throw new Exception("PasswordSaltKey not found");

Configuration.Secrets.JwtToken = jwtToken;
Configuration.Secrets.PasswordSaltKey = passwordSaltKey;

builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddTransient<AuthService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(bearerOptions =>
{
    var jwtTokenBytes = Encoding.ASCII.GetBytes(jwtToken);
    bearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(jwtTokenBytes),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Logged",  policy => policy.RequireAuthenticatedUser());
    options.AddPolicy("Pro",  policy => policy.RequireRole("Pro", "Admin"));
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");
app.AddGets();
app.AddPosts();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();