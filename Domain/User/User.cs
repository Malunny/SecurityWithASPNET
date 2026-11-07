using Domain.Interfaces;

namespace Domain.User;

public class User : IToken
{
    public string Token { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}