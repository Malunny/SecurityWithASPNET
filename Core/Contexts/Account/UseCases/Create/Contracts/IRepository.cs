using Domain.Contexts.Account.Entities.Users;

namespace Domain.Contexts.Account.UseCases.Create.Contracts;

public interface IRepository
{
    Task<bool> AnyAsync(string email, CancellationToken cancellationToken);
    Task SaveAsync(User user, CancellationToken cancellationToken);
}