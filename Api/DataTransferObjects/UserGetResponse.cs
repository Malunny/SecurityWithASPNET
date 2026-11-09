using Domain.User;

namespace SecWithJWTeBearer.DataTransferObjects;

public record UserGetResponse
{
    public string Name;
    public string Email;
    public string[] Roles;

    public UserGetResponse(User user)
    {
        Name = user.Name;
        Email = user.Email;
        Roles = user.Roles.Select(r => r.Name).ToArray();
    }
}