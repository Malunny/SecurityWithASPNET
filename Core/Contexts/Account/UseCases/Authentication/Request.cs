using MediatR;

namespace Domain.Contexts.Account.UseCases.Authentication;

public record Request(
    string Email,
    string Password
) : IRequest<Response>;