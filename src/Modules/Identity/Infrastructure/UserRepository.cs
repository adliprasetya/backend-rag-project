using Identity.Application;
using Identity.Domain;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Identity.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly DbContext _context;
    private readonly DbSet<User> _users;

    public UserRepository(DbContext context)
    {
        _context = context;
        _users = context.Set<User>();
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task AddAsync(User entity, CancellationToken ct = default) =>
        await _users.AddAsync(entity, ct);

    public void Update(User entity) => _users.Update(entity);
    public void Delete(User entity) => _users.Remove(entity);

    public async Task<User?> FindByEmailAsync(string email, CancellationToken ct = default) =>
        await _users.FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), ct);
}
