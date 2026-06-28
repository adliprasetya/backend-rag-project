namespace Document.Application;

public interface ITextExtractionService
{
    Task<string> ExtractTextAsync(string filePath, string contentType, CancellationToken ct = default);
}
