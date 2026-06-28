using Chat.Application;
using Chat.Domain;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure;

public class ChatSessionRepository : IChatSessionRepository
{
    private readonly DbContext _context;
    private readonly DbSet<Domain.ChatSession> _sessions;

    public ChatSessionRepository(DbContext context)
    {
        _context = context;
        _sessions = context.Set<Domain.ChatSession>();
    }

    public async Task<Domain.ChatSession?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _sessions.Include(s => s.Messages).FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task AddAsync(Domain.ChatSession entity, CancellationToken ct = default) =>
        await _sessions.AddAsync(entity, ct);

    public void Update(Domain.ChatSession entity) => _sessions.Update(entity);
    public void Delete(Domain.ChatSession entity) => _sessions.Remove(entity);

    public async Task<List<Domain.ChatSession>> GetByWorkspaceAsync(Guid workspaceId, Guid userId, CancellationToken ct = default) =>
        await _sessions
            .Where(s => s.WorkspaceId == workspaceId && s.UserId == userId)
            .OrderByDescending(s => s.UpdatedAt ?? s.CreatedAt)
            .ToListAsync(ct);

    public async Task<Domain.ChatSession?> GetWithMessagesAsync(Guid id, CancellationToken ct = default) =>
        await _sessions
            .Include(s => s.Messages)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<List<Domain.ChatMessage>> GetMessagesAsync(Guid sessionId, CancellationToken ct = default) =>
        await _context.Set<Domain.ChatMessage>()
            .Where(m => m.SessionId == sessionId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync(ct);

    public async Task AddMessageAsync(Domain.ChatMessage message, CancellationToken ct = default) =>
        await _context.Set<Domain.ChatMessage>().AddAsync(message, ct);
}
