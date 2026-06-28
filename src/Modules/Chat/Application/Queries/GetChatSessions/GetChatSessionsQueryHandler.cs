using Chat.Application.Commands.CreateChatSession;
using MediatR;
using SharedKernel.Domain;

namespace Chat.Application.Queries.GetChatSessions;

public class GetChatSessionsQueryHandler : IRequestHandler<GetChatSessionsQuery, Result<List<ChatSessionResponse>>>
{
    private readonly IChatSessionRepository _chatRepo;

    public GetChatSessionsQueryHandler(IChatSessionRepository chatRepo)
    {
        _chatRepo = chatRepo;
    }

    public async Task<Result<List<ChatSessionResponse>>> Handle(GetChatSessionsQuery request, CancellationToken ct)
    {
        var sessions = await _chatRepo.GetByWorkspaceAsync(request.WorkspaceId, request.UserId, ct);

        var response = sessions.Select(s => new ChatSessionResponse(
            s.Id, s.Title, s.WorkspaceId, s.CreatedAt)).ToList();

        return Result.Success(response);
    }
}
