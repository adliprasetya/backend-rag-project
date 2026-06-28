using SharedKernel;

namespace Document.Application;

public interface IDocumentRepository : IRepository<Domain.Document>
{
    Task<List<Domain.Document>> GetByWorkspaceAsync(Guid workspaceId, CancellationToken ct = default);
    Task<List<Domain.Document>> GetByStatusAsync(Domain.DocumentStatus status, CancellationToken ct = default);
    Task<Domain.Document?> GetWithChunksAsync(Guid id, CancellationToken ct = default);
}
