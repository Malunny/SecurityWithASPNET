using System.Text.RegularExpressions;
using Domain.Contexts.Shared;
using Domain.Contexts.Shared.Extensions;

namespace Domain.Contexts.Account.ValueObjects;

public partial class Email : ValueObject
{
    private const string Pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

    public string Address { get; }
    public string Hash => Address.ToBase64();
    public Verification Verification { get; private set; } = new();
    
    protected Email() { }

    public Email(string address)
    {
        if (string.IsNullOrWhiteSpace(address)) 
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(address));
        
        if (address.Length < 5)
            throw new ArgumentException("Value must contain at least 5 characters.", nameof(address));
        
        if (!EmailRegex().IsMatch(address))
            throw new ArgumentException("Invalid email address.", nameof(address));
        
        Address = address;
    }
    
    public void ResendVerification()
    {
        Verification = new Verification();
    }
    
    public override string ToString() => Address;
    
    public static implicit operator string(Email email) => email.ToString();
    public static implicit operator Email(string address) => new Email(address);
    
    [GeneratedRegex(Pattern)]
    private static partial Regex EmailRegex();
}