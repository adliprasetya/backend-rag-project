using SharedKernel.Domain;

namespace Chat.Domain;

public class ChatSession : BaseEntity
{
    private readonly List<ChatMessage> _messages = [];

    public string Title { get; private set; } = string.Empty;
    public Guid WorkspaceId { get; private set; }
    public Guid UserId { get; private set; }
    public IReadOnlyCollection<ChatMessage> Messages => _messages.AsReadOnly();

    private ChatSession() { }

    public static ChatSession Create(string title, Guid workspaceId, Guid userId)
    {
        return new ChatSession
        {
            Title = title,
            WorkspaceId = workspaceId,
            UserId = userId,
        };
    }

    public void AddMessage(ChatMessage message)
    {
        _messages.Add(message);
        Touch();
    }

    public void UpdateTitle(string title)
    {
        Title = title;
        Touch();
    }
}
