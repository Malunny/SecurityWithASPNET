using Domain.Contexts.Account.Entities.Roles;
using Domain.Contexts.Account.ValueObjects;
using Domain.Contexts.Shared;
using Domain.Interfaces;

namespace Domain.Contexts.Account.Entities.Users;

public class User : Entity
{
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