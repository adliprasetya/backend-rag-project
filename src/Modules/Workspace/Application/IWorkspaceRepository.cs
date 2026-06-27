using SharedKernel;

namespace Workspace.Application;

public interface IWorkspaceRepository : IRepository<Domain.Workspace>
{
    Task<List<Domain.Workspace>> GetUserWorkspacesAsync(Guid userId, CancellationToken ct = default);
}
