using Microsoft.EntityFrameworkCore;
using Workspace.Application;

namespace Workspace.Infrastructure;

public class WorkspaceRepository : IWorkspaceRepository
{
    private readonly DbContext _context;
    private readonly DbSet<Domain.Workspace> _workspaces;

    public WorkspaceRepository(DbContext context)
    {
        _context = context;
        _workspaces = context.Set<Domain.Workspace>();
    }

    public async Task<Domain.Workspace?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _workspaces.Include(w => w.Members).FirstOrDefaultAsync(w => w.Id == id, ct);

    public async Task AddAsync(Domain.Workspace entity, CancellationToken ct = default) =>
        await _workspaces.AddAsync(entity, ct);

    public void Update(Domain.Workspace entity) => _workspaces.Update(entity);
    public void Delete(Domain.Workspace entity) => _workspaces.Remove(entity);

    public async Task<List<Domain.Workspace>> GetUserWorkspacesAsync(Guid userId, CancellationToken ct = default) =>
        await _workspaces
            .Include(w => w.Members)
            .Where(w => w.OwnerId == userId || w.Members.Any(m => m.UserId == userId))
            .ToListAsync(ct);
}
