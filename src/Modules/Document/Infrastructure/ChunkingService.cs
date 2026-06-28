using Document.Application;

namespace Document.Infrastructure;

public class ChunkingService : IChunkingService
{
    public List<ChunkResult> ChunkText(string text, int chunkSize = 512, int overlap = 64)
    {
        if (string.IsNullOrWhiteSpace(text))
            return [];

        var results = new List<ChunkResult>();
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var index = 0;

        for (int i = 0; i < words.Length; i += chunkSize - overlap)
        {
            var chunkWords = words.Skip(i).Take(chunkSize).ToArray();
            if (chunkWords.Length == 0) break;

            var content = string.Join(' ', chunkWords);
            var tokenCount = EstimateTokenCount(content);

            results.Add(new ChunkResult(content, tokenCount, index));
            index++;

            if (i + chunkSize >= words.Length) break;
        }

        return results;
    }

    private static int EstimateTokenCount(string text)
    {
        return text.Length / 4;
    }
}
