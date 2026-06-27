using MediatR;
using SharedKernel.Domain;

namespace Workspace.Application.Commands.CreateWorkspace;

public record CreateWorkspaceCommand(string Name, string? Description, Guid OwnerId) : IRequest<Result<WorkspaceResponse>>;

public record WorkspaceResponse(Guid Id, string Name, string? Description, Guid OwnerId, DateTime CreatedAt);
