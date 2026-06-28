using SharedKernel.Domain;

namespace Document.Domain;

public class Document : BaseEntity
{
    private readonly List<DocumentChunk> _chunks = [];

    public string Name { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public long FileSize { get; private set; }
    public string StoragePath { get; private set; } = string.Empty;
    public Guid WorkspaceId { get; private set; }
    public Guid UploadedBy { get; private set; }
    public DocumentStatus Status { get; private set; } = DocumentStatus.Pending;
    public string? ErrorMessage { get; private set; }
    public IReadOnlyCollection<DocumentChunk> Chunks => _chunks.AsReadOnly();

    private Document() { }

    public static Document Create(string name, string contentType, long fileSize, string storagePath, Guid workspaceId, Guid uploadedBy)
    {
        return new Document
        {
            Name = name,
            ContentType = contentType,
            FileSize = fileSize,
            StoragePath = storagePath,
            WorkspaceId = workspaceId,
            UploadedBy = uploadedBy,
        };
    }

    public void SetStatus(DocumentStatus status)
    {
        Status = status;
        Touch();
    }

    public void SetError(string message)
    {
        Status = DocumentStatus.Failed;
        ErrorMessage = message;
        Touch();
    }

    public void AddChunk(DocumentChunk chunk)
    {
        _chunks.Add(chunk);
        Touch();
    }

    public void ClearChunks()
    {
        _chunks.Clear();
        Touch();
    }
}
