using MediatR;
using SharedKernel.Domain;
using Workspace.Application.Commands.CreateWorkspace;

namespace Workspace.Application.Queries.GetWorkspaces;

public class GetWorkspacesQueryHandler : IRequestHandler<GetWorkspacesQuery, Result<List<WorkspaceResponse>>>
{
    private readonly IWorkspaceRepository _workspaceRepo;

    public GetWorkspacesQueryHandler(IWorkspaceRepository workspaceRepo)
    {
        _workspaceRepo = workspaceRepo;
    }

    public async Task<Result<List<WorkspaceResponse>>> Handle(GetWorkspacesQuery request, CancellationToken ct)
    {
        var workspaces = await _workspaceRepo.GetUserWorkspacesAsync(request.UserId, ct);

        var response = workspaces.Select(w => new WorkspaceResponse(
            w.Id, w.Name, w.Description, w.OwnerId, w.CreatedAt)).ToList();

        return Result.Success(response);
    }
}
