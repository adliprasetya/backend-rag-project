using Chat.Application.Commands.SendMessage;
using MediatR;
using SharedKernel.Domain;

namespace Chat.Application.Queries.GetChatMessages;

public record GetChatMessagesQuery(Guid SessionId) : IRequest<Result<List<MessageResponse>>>;
