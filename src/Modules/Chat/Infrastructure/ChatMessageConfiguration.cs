using Chat.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Infrastructure;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable("ChatMessages");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Content).HasColumnType("text").IsRequired();
        builder.Property(m => m.Role)
            .HasConversion<int>()
            .IsRequired();

        builder.HasIndex(m => m.SessionId);
    }
}
