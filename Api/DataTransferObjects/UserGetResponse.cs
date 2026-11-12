using Domain.Entities.Users;

namespace SecWithJWTeBearer.DataTransferObjects;

public record UserGetResponse
{
    public string Name { get; }
    public string Email { get; }
    public string[] Roles { get; }

    public UserGetResponse(User user)
    {
        Name = user.Name;
        Email = user.Email;
        Roles = user.Roles.Select(r => r.Name).ToArray();
    }
}