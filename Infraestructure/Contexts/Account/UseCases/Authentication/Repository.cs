using Domain.Contexts.Account.Entities.Users;
using Microsoft.EntityFrameworkCore;
using SecWithJWTeBearer;

namespace Infraestructure.Contexts.Account.UseCases.Authentication;

public class Repository : Domain.Contexts.Account.UseCases.Authentication.Contracts.IRepository
{
    private readonly DatabaseContext _databaseContext;

    public Repository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    public async Task<User?> GetUserByEmailAsync(string requestEmail, CancellationToken cancellationToken)
    {
        return await _databaseContext.Users.FirstOrDefaultAsync(user => user.Email.Address == requestEmail, cancellationToken); 
    }
}