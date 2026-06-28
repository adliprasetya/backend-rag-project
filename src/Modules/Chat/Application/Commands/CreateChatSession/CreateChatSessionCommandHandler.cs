using Chat.Domain;
using MediatR;
using SharedKernel;
using SharedKernel.Domain;

namespace Chat.Application.Commands.CreateChatSession;

public class CreateChatSessionCommandHandler : IRequestHandler<CreateChatSessionCommand, Result<ChatSessionResponse>>
{
    private readonly IChatSessionRepository _chatRepo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateChatSessionCommandHandler(IChatSessionRepository chatRepo, IUnitOfWork unitOfWork)
    {
        _chatRepo = chatRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ChatSessionResponse>> Handle(CreateChatSessionCommand request, CancellationToken ct)
    {
        var session = Domain.ChatSession.Create(
            request.Title,
            request.WorkspaceId,
            request.UserId);

        await _chatRepo.AddAsync(session, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success(new ChatSessionResponse(
            session.Id,
            session.Title,
            session.WorkspaceId,
            session.CreatedAt));
    }
}
