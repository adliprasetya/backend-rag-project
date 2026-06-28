using Document.Application;

namespace Document.Infrastructure;

public class EmbeddingService : IEmbeddingService
{
    public Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken ct = default)
    {
        // Placeholder — will be replaced by AI Module in Phase 8
        var mockEmbedding = new float[1536];
        var random = new Random(text.GetHashCode());
        for (int i = 0; i < mockEmbedding.Length; i++)
            mockEmbedding[i] = (float)(random.NextDouble() * 2 - 1);

        return Task.FromResult(mockEmbedding);
    }

    public async Task<List<float[]>> GenerateEmbeddingsAsync(List<string> texts, CancellationToken ct = default)
    {
        var results = new List<float[]>();
        foreach (var text in texts)
        {
            var embedding = await GenerateEmbeddingAsync(text, ct);
            results.Add(embedding);
        }
        return results;
    }
}
