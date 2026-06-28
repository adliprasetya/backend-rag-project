using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workspace.Application.Commands.CreateWorkspace;
using Workspace.Application.Queries.GetWorkspaces;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/workspaces")]
public class WorkspaceController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkspaceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkspaceCommand command, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null || !Guid.TryParse(userId, out var id))
            return Unauthorized();

        var cmd = command with { OwnerId = id };
        var result = await _mediator.Send(cmd, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(new { error = result.Error });
    }

    [HttpGet]
    public async Task<IActionResult> GetMyWorkspaces(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null || !Guid.TryParse(userId, out var id))
            return Unauthorized();

        var result = await _mediator.Send(new GetWorkspacesQuery(id), ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(new { error = result.Error });
    }
}
