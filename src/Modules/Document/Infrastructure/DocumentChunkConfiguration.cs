using System.Text.Json;
using Document.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Document.Infrastructure;

public class DocumentChunkConfiguration : IEntityTypeConfiguration<DocumentChunk>
{
    public void Configure(EntityTypeBuilder<DocumentChunk> builder)
    {
        builder.ToTable("DocumentChunks");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Content).HasColumnType("text").IsRequired();
        builder.Property(c => c.Index).IsRequired();
        builder.Property(c => c.TokenCount).IsRequired();
        builder.Property(c => c.Embedding)
            .HasColumnType("jsonb")
            .HasConversion(new EmbeddingConverter());

        builder.HasIndex(c => c.DocumentId);
    }
}

public class EmbeddingConverter : ValueConverter<float[]?, string?>
{
    public EmbeddingConverter()
        : base(
            v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => v == null ? null : JsonSerializer.Deserialize<float[]>(v, (JsonSerializerOptions?)null))
    {
    }
}
