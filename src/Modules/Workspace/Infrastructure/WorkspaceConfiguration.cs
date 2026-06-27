using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workspace.Domain;

namespace Workspace.Infrastructure;

public class WorkspaceConfiguration : IEntityTypeConfiguration<Domain.Workspace>
{
    public void Configure(EntityTypeBuilder<Domain.Workspace> builder)
    {
        builder.ToTable("Workspaces");
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Name).HasMaxLength(200).IsRequired();
        builder.Property(w => w.Description).HasMaxLength(2000);
        builder.Property(w => w.OwnerId).IsRequired();

        builder.HasMany(w => w.Members)
            .WithOne()
            .HasForeignKey("WorkspaceId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
