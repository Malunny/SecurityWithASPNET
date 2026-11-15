using Domain.Contexts.Account.Entities.Users;

namespace Domain.Contexts.Account.UseCases.Create.Contracts;

public interface IService
{
    Task SendVerificationEmailAsync(User user, CancellationToken cancellationToken);
}