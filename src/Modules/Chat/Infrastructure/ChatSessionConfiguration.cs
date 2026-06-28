using Chat.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Infrastructure;

public class ChatSessionConfiguration : IEntityTypeConfiguration<Domain.ChatSession>
{
    public void Configure(EntityTypeBuilder<Domain.ChatSession> builder)
    {
        builder.ToTable("ChatSessions");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Title).HasMaxLength(500).IsRequired();
        builder.Property(s => s.WorkspaceId).IsRequired();
        builder.Property(s => s.UserId).IsRequired();

        builder.HasMany(s => s.Messages)
            .WithOne()
            .HasForeignKey("SessionId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.WorkspaceId);
        builder.HasIndex(s => s.UserId);
    }
}
