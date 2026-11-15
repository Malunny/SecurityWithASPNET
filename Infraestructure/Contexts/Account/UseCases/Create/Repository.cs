using Domain.Contexts.Account.Entities.Users;
using Domain.Contexts.Account.UseCases.Create.Contracts;
using Microsoft.EntityFrameworkCore;
using SecWithJWTeBearer;

namespace Infraestructure.Contexts.Account.UseCases.Create;

public class Repository : IRepository
{
    private readonly DatabaseContext  _context;

    public Repository(DatabaseContext context)
    {
        _context = context;
    }
    public async Task<bool> AnyAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users.AsNoTracking().AnyAsync(user => user.Email.Address == email, cancellationToken);
    }

    public async Task SaveAsync(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}