using Chat.Application.Commands.CreateChatSession;
using MediatR;
using SharedKernel.Domain;

namespace Chat.Application.Queries.GetChatSessions;

public record GetChatSessionsQuery(Guid WorkspaceId, Guid UserId) : IRequest<Result<List<ChatSessionResponse>>>;
