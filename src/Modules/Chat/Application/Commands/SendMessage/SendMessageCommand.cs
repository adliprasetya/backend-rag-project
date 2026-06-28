using MediatR;
using SharedKernel.Domain;

namespace Chat.Application.Commands.SendMessage;

public record SendMessageCommand(
    Guid SessionId,
    Guid UserId,
    string Content) : IRequest<Result<MessageResponse>>;

public record MessageResponse(Guid Id, Guid SessionId, string Role, string Content, DateTime CreatedAt);
