using Identity.Domain;
using SharedKernel;

namespace Identity.Application;

public interface IUserRepository : IRepository<User>
{
    Task<User?> FindByEmailAsync(string email, CancellationToken ct = default);
}
