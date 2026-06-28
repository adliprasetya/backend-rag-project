using Document.Application;
using Microsoft.EntityFrameworkCore;

namespace Document.Infrastructure;

public class DocumentRepository : IDocumentRepository
{
    private readonly DbContext _context;
    private readonly DbSet<Domain.Document> _documents;

    public DocumentRepository(DbContext context)
    {
        _context = context;
        _documents = context.Set<Domain.Document>();
    }

    public async Task<Domain.Document?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _documents.Include(d => d.Chunks).FirstOrDefaultAsync(d => d.Id == id, ct);

    public async Task AddAsync(Domain.Document entity, CancellationToken ct = default) =>
        await _documents.AddAsync(entity, ct);

    public void Update(Domain.Document entity) => _documents.Update(entity);
    public void Delete(Domain.Document entity) => _documents.Remove(entity);

    public async Task<List<Domain.Document>> GetByWorkspaceAsync(Guid workspaceId, CancellationToken ct = default) =>
        await _documents
            .Where(d => d.WorkspaceId == workspaceId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(ct);

    public async Task<List<Domain.Document>> GetByStatusAsync(Domain.DocumentStatus status, CancellationToken ct = default) =>
        await _documents
            .Where(d => d.Status == status)
            .ToListAsync(ct);

    public async Task<Domain.Document?> GetWithChunksAsync(Guid id, CancellationToken ct = default) =>
        await _documents
            .Include(d => d.Chunks)
            .FirstOrDefaultAsync(d => d.Id == id, ct);
}
