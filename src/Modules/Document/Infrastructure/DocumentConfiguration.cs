using Document.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Document.Infrastructure;

public class DocumentConfiguration : IEntityTypeConfiguration<Domain.Document>
{
    public void Configure(EntityTypeBuilder<Domain.Document> builder)
    {
        builder.ToTable("Documents");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).HasMaxLength(500).IsRequired();
        builder.Property(d => d.ContentType).HasMaxLength(200).IsRequired();
        builder.Property(d => d.FileSize).IsRequired();
        builder.Property(d => d.StoragePath).HasMaxLength(2000).IsRequired();
        builder.Property(d => d.WorkspaceId).IsRequired();
        builder.Property(d => d.UploadedBy).IsRequired();
        builder.Property(d => d.Status)
            .HasConversion<int>()
            .IsRequired();
        builder.Property(d => d.ErrorMessage).HasMaxLength(2000);

        builder.HasMany(d => d.Chunks)
            .WithOne()
            .HasForeignKey("DocumentId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(d => d.WorkspaceId);
        builder.HasIndex(d => d.Status);
    }
}
