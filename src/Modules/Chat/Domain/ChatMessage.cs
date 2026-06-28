namespace Chat.Domain;

public enum MessageRole
{
    User,
    Assistant,
    System,
}

public class ChatMessage
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid SessionId { get; private set; }
    public MessageRole Role { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private ChatMessage() { }

    public static ChatMessage Create(Guid sessionId, MessageRole role, string content)
    {
        return new ChatMessage
        {
            SessionId = sessionId,
            Role = role,
            Content = content,
        };
    }
}
