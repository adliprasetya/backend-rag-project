namespace Document.Domain;

public enum DocumentStatus
{
    Pending,
    Extracting,
    Chunking,
    Embedding,
    Ready,
    Failed,
}
