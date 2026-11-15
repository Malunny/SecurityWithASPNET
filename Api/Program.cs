using System.Text;
using Domain;
using Infraestructure.Contexts.Account.UseCases.Create;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecWithJWTeBearer;
using SecWithJWTeBearer.DataTransferObjects;
using SecWithJWTeBearer.Extensions;
using SecWithJWTeBearer.Extensions.Contexts;
using SecWithJWTeBearer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.AddConfiguration();
builder.AddDatabase();
builder.AddEmails();

builder.AddAccountContext();
builder.AddMediator();
builder.Services.AddTransient<AuthService>();

builder.AddJwtAuthentication();
builder.AddAuthorizationPolicies();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.AddGets();
app.MapAccountEndpoints();  
app.AddPosts();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();