using Domain.Entities.Users;
using Domain.Interfaces;

namespace Domain.Entities.Roles;

public class Role : IToken
{
    public string Token { get; set; }
    public string Name { get; set; }

    public List<User> Users { get; set; }
}