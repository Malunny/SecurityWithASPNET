using Domain.Contexts.Account.Entities.Users;
using Domain.Contexts.Shared;
using Domain.Interfaces;

namespace Domain.Contexts.Account.Entities.Roles;

public class Role : Entity
{
    public string Name { get; set; }

    public List<User> Users { get; set; }
}