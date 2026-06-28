namespace Document.Application;

public interface IChunkingService
{
    List<ChunkResult> ChunkText(string text, int chunkSize = 512, int overlap = 64);
}

public record ChunkResult(string Content, int TokenCount, int Index);
