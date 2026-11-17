using Domain.Contexts.Account.Entities.Users;

namespace Domain.Contexts.Account.UseCases.Authentication.Contracts;

public interface IRepository
{
    Task<User?> GetUserByEmailAsync(string requestEmail, CancellationToken cancellationToken);
}