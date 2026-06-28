using Document.Application;

namespace Document.Infrastructure;

public class TextExtractionService : ITextExtractionService
{
    public async Task<string> ExtractTextAsync(string filePath, string contentType, CancellationToken ct = default)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();

        return extension switch
        {
            ".txt" => await File.ReadAllTextAsync(filePath, ct),
            ".md" => await File.ReadAllTextAsync(filePath, ct),
            ".pdf" => await ExtractPdfTextAsync(filePath, ct),
            ".docx" => await ExtractDocxTextAsync(filePath, ct),
            _ => throw new NotSupportedException($"File type '{extension}' is not supported"),
        };
    }

    private static Task<string> ExtractPdfTextAsync(string filePath, CancellationToken ct)
    {
        // Basic PDF text extraction fallback
        return Task.FromResult($"[PDF extraction not yet implemented: {Path.GetFileName(filePath)}]");
    }

    private static Task<string> ExtractDocxTextAsync(string filePath, CancellationToken ct)
    {
        return Task.FromResult($"[DOCX extraction not yet implemented: {Path.GetFileName(filePath)}]");
    }
}
