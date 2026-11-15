using MediatR;

namespace Domain.Contexts.Account.UseCases.Create;

public record Request(string Name, string Email, string Password) : IRequest<Response>;