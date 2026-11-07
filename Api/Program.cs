using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecWithJWTeBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration["ConnectionString"];

builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/users", ([FromServices] DatabaseContext db) => db.Users.ToList());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();