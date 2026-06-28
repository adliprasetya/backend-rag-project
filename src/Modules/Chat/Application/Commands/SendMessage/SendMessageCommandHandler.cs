using Chat.Domain;
using MediatR;
using SharedKernel;
using SharedKernel.Domain;

namespace Chat.Application.Commands.SendMessage;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Result<MessageResponse>>
{
    private readonly IChatSessionRepository _chatRepo;
    private readonly IUnitOfWork _unitOfWork;

    public SendMessageCommandHandler(IChatSessionRepository chatRepo, IUnitOfWork unitOfWork)
    {
        _chatRepo = chatRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<MessageResponse>> Handle(SendMessageCommand request, CancellationToken ct)
    {
        var session = await _chatRepo.GetByIdAsync(request.SessionId, ct);
        if (session is null)
            return Result.Failure<MessageResponse>(Error.NotFound("Chat session not found"));

        var message = ChatMessage.Create(request.SessionId, MessageRole.User, request.Content);
        await _chatRepo.AddMessageAsync(message, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success(new MessageResponse(
            message.Id,
            message.SessionId,
            message.Role.ToString(),
            message.Content,
            message.CreatedAt));
    }
}
