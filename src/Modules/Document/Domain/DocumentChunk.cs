namespace Document.Domain;

public class DocumentChunk
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid DocumentId { get; private set; }
    public int Index { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public int TokenCount { get; private set; }
    public float[]? Embedding { get; set; }

    private DocumentChunk() { }

    public static DocumentChunk Create(Guid documentId, int index, string content, int tokenCount)
    {
        return new DocumentChunk
        {
            DocumentId = documentId,
            Index = index,
            Content = content,
            TokenCount = tokenCount,
        };
    }
}
