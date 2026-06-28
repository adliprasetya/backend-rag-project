namespace Document.Application;

public interface IEmbeddingService
{
    Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken ct = default);
    Task<List<float[]>> GenerateEmbeddingsAsync(List<string> texts, CancellationToken ct = default);
}
