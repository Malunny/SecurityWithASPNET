using Domain.Interfaces;

namespace Domain.Contexts.Shared;

public class Entity : IToken
{
    public string Token { get; } = Guid.NewGuid().ToString();
}