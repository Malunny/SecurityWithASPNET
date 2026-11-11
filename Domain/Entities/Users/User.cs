using Domain.Entities.Roles;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Entities.Users;

public class User : IToken
{
    public string Token { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public Email Email { get; private set; }
    public Password Password { get; private set; }
    public List<Role> Roles { get; set; } = new();

    public void GeneratePassword(string plainTextPassword, string code)
    {
        if (!string.Equals(code.Trim(), Password.ResetCode.Trim(), StringComparison.InvariantCultureIgnoreCase))
            throw new Exception("Reset codes do not match");
        
        var password = new Password(plainTextPassword);
        Password = password;
    }

    public void UpdateEmail(Email email)
    {
        Email = email;
    }

    public void ChangePassword(string plainTextPassword)
    {
        var password = new Password(plainTextPassword);
        Password = password;
    }
    
    protected User() { }
    public User(string email, string? password = null)
    {
        Email = email;
        Password = new Password(password);
    }
    public User(string name, string email, string? password = null)
    {
        Name = name;
        Email = email;
        Password = new Password(password);
    }
}