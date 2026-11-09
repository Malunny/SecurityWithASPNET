using Domain.Interfaces;

namespace Domain.Role;

public class Role : IToken
{
    public string Token { get; set; }
    public string Name { get; set; }

    public List<User.User> Users { get; set; }
}