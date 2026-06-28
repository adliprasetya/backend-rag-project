using System.Security.Claims;
using Chat.Application.Commands.CreateChatSession;
using Chat.Application.Commands.SendMessage;
using Chat.Application.Queries.GetChatMessages;
using Chat.Application.Queries.GetChatSessions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sessions")]
    public async Task<IActionResult> CreateSession([FromBody] CreateChatSessionCommand command, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null || !Guid.TryParse(userId, out var id))
            return Unauthorized();

        var cmd = command with { UserId = id };
        var result = await _mediator.Send(cmd, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(new { error = result.Error });
    }

    [HttpGet("sessions")]
    public async Task<IActionResult> GetSessions([FromQuery] Guid workspaceId, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null || !Guid.TryParse(userId, out var id))
            return Unauthorized();

        var result = await _mediator.Send(new GetChatSessionsQuery(workspaceId, id), ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(new { error = result.Error });
    }

    [HttpPost("sessions/{sessionId:guid}/messages")]
    public async Task<IActionResult> SendMessage(Guid sessionId, [FromBody] SendMessageBody body, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null || !Guid.TryParse(userId, out var id))
            return Unauthorized();

        var command = new SendMessageCommand(sessionId, id, body.Content);
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(new { error = result.Error });
    }

    [HttpGet("sessions/{sessionId:guid}/messages")]
    public async Task<IActionResult> GetMessages(Guid sessionId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetChatMessagesQuery(sessionId), ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(new { error = result.Error });
    }
}

public record SendMessageBody(string Content);
