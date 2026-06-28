using System.Security.Claims;
using Document.Application.Commands.UploadDocument;
using Document.Application.Queries.GetDocumentById;
using Document.Application.Queries.GetDocuments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/documents")]
public class DocumentController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("upload")]
    [RequestSizeLimit(52_428_800)]
    public async Task<IActionResult> Upload(IFormFile file, Guid workspaceId, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null || !Guid.TryParse(userId, out var id))
            return Unauthorized();

        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(uploadsDir, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, ct);
        }

        var command = new UploadDocumentCommand(
            file.FileName,
            file.ContentType,
            file.Length,
            filePath,
            workspaceId,
            id);

        var result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(new { error = result.Error });
    }

    [HttpGet]
    public async Task<IActionResult> GetByWorkspace([FromQuery] Guid workspaceId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDocumentsQuery(workspaceId), ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(new { error = result.Error });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDocumentByIdQuery(id), ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(new { error = result.Error });
    }
}
