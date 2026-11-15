using Flunt.Notifications;
using Flunt.Validations;

namespace Domain.Contexts.Account.UseCases.Create;

public static class Specification
{
    public static Contract<Notification> Ensure(Request request)
        => new Contract<Notification>()
            .Requires()
            .IsLowerThan(request.Name.Length, 160, "Name", "Name size must be lower than 160")
            .IsGreaterThan(request.Name.Length, 3, "Name", "Name must be greater than 3 characters")
            .IsLowerThan(request.Password.Length, 40, "Password", "Password size must have be lower than 40 characters")
            .IsGreaterThan(request.Password.Length, 7, "Password", "Password must have at least 8 characters")
            .IsEmail(request.Email, "Email", "Invalid email");
}