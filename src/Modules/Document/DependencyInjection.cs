using System.Reflection;
using Document.Application;
using Document.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Document;

public static class DependencyInjection
{
    public static IServiceCollection AddDocumentModule(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<ITextExtractionService, TextExtractionService>();
        services.AddScoped<IChunkingService, ChunkingService>();
        services.AddScoped<IEmbeddingService, EmbeddingService>();
        return services;
    }
}
