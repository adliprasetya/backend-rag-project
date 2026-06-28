using MediatR;
using SharedKernel.Domain;

namespace Chat.Application.Commands.CreateChatSession;

public record CreateChatSessionCommand(
    string Title,
    Guid WorkspaceId,
    Guid UserId) : IRequest<Result<ChatSessionResponse>>;

public record ChatSessionResponse(Guid Id, string Title, Guid WorkspaceId, DateTime CreatedAt);
