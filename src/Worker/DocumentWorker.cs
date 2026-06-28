using Document.Application;
using Document.Domain;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Worker;

public class DocumentWorker : BackgroundService
{
    private readonly ILogger<DocumentWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public DocumentWorker(ILogger<DocumentWorker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Document worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                await ProcessPendingDocumentsAsync(scope, stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error processing documents");
            }

            await Task.Delay(5000, stoppingToken);
        }
    }

    private async Task ProcessPendingDocumentsAsync(IServiceScope scope, CancellationToken ct)
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var textExtractor = scope.ServiceProvider.GetRequiredService<ITextExtractionService>();
        var chunkingService = scope.ServiceProvider.GetRequiredService<IChunkingService>();
        var embeddingService = scope.ServiceProvider.GetRequiredService<IEmbeddingService>();

        var pendingDocs = await context.Documents
            .Where(d => d.Status == DocumentStatus.Pending)
            .ToListAsync(ct);

        foreach (var doc in pendingDocs)
        {
            try
            {
                _logger.LogInformation("Processing document {Id}: {Name}", doc.Id, doc.Name);

                // Step 1: Extract text
                doc.SetStatus(DocumentStatus.Extracting);
                await context.SaveChangesAsync(ct);

                var text = await textExtractor.ExtractTextAsync(doc.StoragePath, doc.ContentType, ct);

                // Step 2: Chunk text
                doc.SetStatus(DocumentStatus.Chunking);
                await context.SaveChangesAsync(ct);

                var chunks = chunkingService.ChunkText(text);

                // Step 3: Generate embeddings
                doc.SetStatus(DocumentStatus.Embedding);
                await context.SaveChangesAsync(ct);

                foreach (var chunk in chunks)
                {
                    var embedding = await embeddingService.GenerateEmbeddingAsync(chunk.Content, ct);
                    var docChunk = DocumentChunk.Create(doc.Id, chunk.Index, chunk.Content, chunk.TokenCount);
                    docChunk.Embedding = embedding;
                    doc.AddChunk(docChunk);
                }

                // Step 4: Mark as ready
                doc.SetStatus(DocumentStatus.Ready);
                await context.SaveChangesAsync(ct);

                _logger.LogInformation("Document {Id} processed successfully ({ChunkCount} chunks)", doc.Id, chunks.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process document {Id}", doc.Id);
                doc.SetError(ex.Message);
                await context.SaveChangesAsync(ct);
            }
        }
    }
}
