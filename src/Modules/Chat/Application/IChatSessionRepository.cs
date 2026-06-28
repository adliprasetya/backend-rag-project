using SharedKernel;

namespace Chat.Application;

public interface IChatSessionRepository : IRepository<Domain.ChatSession>
{
    Task<List<Domain.ChatSession>> GetByWorkspaceAsync(Guid workspaceId, Guid userId, CancellationToken ct = default);
    Task<Domain.ChatSession?> GetWithMessagesAsync(Guid id, CancellationToken ct = default);
    Task<List<Domain.ChatMessage>> GetMessagesAsync(Guid sessionId, CancellationToken ct = default);
    Task AddMessageAsync(Domain.ChatMessage message, CancellationToken ct = default);
}
