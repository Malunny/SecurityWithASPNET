using Domain.Contexts.Account.UseCases.Create;
using Domain.Contexts.Account.UseCases.Create.Contracts;
using Infraestructure.Contexts.Account.UseCases.Create;
using MediatR;

namespace SecWithJWTeBearer.Extensions.Contexts;

public static class AccountContextExtensions
{
    public static void AddAccountContext(this WebApplicationBuilder builder)
    {
        #region Create
        builder.Services.AddTransient<
            Domain.Contexts.Account.UseCases.Create.Contracts.IService, 
            Infraestructure.Contexts.Account.UseCases.Create.Service>();
        builder.Services.AddTransient<
            Domain.Contexts.Account.UseCases.Create.Contracts.IRepository,
            Infraestructure.Contexts.Account.UseCases.Create.Repository
        >();
        #endregion
        #region Auth 
        builder.Services.AddTransient<
            Domain.Contexts.Account.UseCases.Authentication.Contracts.IRepository, 
            Infraestructure.Contexts.Account.UseCases.Authentication.Repository>();
        #endregion
    }
    
    public static void MapAccountEndpoints(this WebApplication app)
    {
        #region Create

        app.MapPost("api/v1/users/register", async (
            Domain.Contexts.Account.UseCases.Create.Request request,
            IRequestHandler<Domain.Contexts.Account.UseCases.Create.Request, 
                Domain.Contexts.Account.UseCases.Create.Response> requestHandler) =>
        {
            var result = await requestHandler.Handle(request, new CancellationToken());
            
            return result.IsSuccess ? Results.Created($"api/v1/users/register", request) 
                : Results.Json(result, statusCode: result.Status);
        }).AllowAnonymous();

        #endregion

        #region Auth

        app.MapPost("api/v1/users/authenticate",
            async (Domain.Contexts.Account.UseCases.Authentication.Request request,
                IRequestHandler<Domain.Contexts.Account.UseCases.Authentication.Request, 
                Domain.Contexts.Account.UseCases.Authentication.Response> requestHandler) =>
            {
                var result = await requestHandler.Handle(request, new CancellationToken());

                return result.IsSuccess ? Results.Ok(result) : Results.Json(result, statusCode: result.Status);
            });

        #endregion
    }
}