using MediatR;
using SharedKernel.Domain;
using Workspace.Application.Commands.CreateWorkspace;

namespace Workspace.Application.Queries.GetWorkspaces;

public record GetWorkspacesQuery(Guid UserId) : IRequest<Result<List<WorkspaceResponse>>>;
