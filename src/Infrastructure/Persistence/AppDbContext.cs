using Chat.Infrastructure;
using Document.Infrastructure;
using Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using SharedKernel.Domain;
using Workspace.Infrastructure;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext, IUnitOfWork
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Workspace.Domain.Workspace> Workspaces => Set<Workspace.Domain.Workspace>();
    public DbSet<Document.Domain.Document> Documents => Set<Document.Domain.Document>();
    public DbSet<Chat.Domain.ChatSession> ChatSessions => Set<Chat.Domain.ChatSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new WorkspaceConfiguration());
        modelBuilder.ApplyConfiguration(new WorkspaceMemberConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentChunkConfiguration());
        modelBuilder.ApplyConfiguration(new ChatSessionConfiguration());
        modelBuilder.ApplyConfiguration(new ChatMessageConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Modified) entry.Entity.Touch();
        }
        return await base.SaveChangesAsync(ct);
    }
}
