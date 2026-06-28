using Chat.Application.Commands.SendMessage;
using MediatR;
using SharedKernel.Domain;

namespace Chat.Application.Queries.GetChatMessages;

public class GetChatMessagesQueryHandler : IRequestHandler<GetChatMessagesQuery, Result<List<MessageResponse>>>
{
    private readonly IChatSessionRepository _chatRepo;

    public GetChatMessagesQueryHandler(IChatSessionRepository chatRepo)
    {
        _chatRepo = chatRepo;
    }

    public async Task<Result<List<MessageResponse>>> Handle(GetChatMessagesQuery request, CancellationToken ct)
    {
        var messages = await _chatRepo.GetMessagesAsync(request.SessionId, ct);

        var response = messages.Select(m => new MessageResponse(
            m.Id, m.SessionId, m.Role.ToString(), m.Content, m.CreatedAt)).ToList();

        return Result.Success(response);
    }
}
