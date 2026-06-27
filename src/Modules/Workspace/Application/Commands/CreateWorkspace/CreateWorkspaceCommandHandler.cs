using MediatR;
using SharedKernel;
using SharedKernel.Domain;

namespace Workspace.Application.Commands.CreateWorkspace;

public class CreateWorkspaceCommandHandler : IRequestHandler<CreateWorkspaceCommand, Result<WorkspaceResponse>>
{
    private readonly IWorkspaceRepository _workspaceRepo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWorkspaceCommandHandler(IWorkspaceRepository workspaceRepo, IUnitOfWork unitOfWork)
    {
        _workspaceRepo = workspaceRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<WorkspaceResponse>> Handle(CreateWorkspaceCommand request, CancellationToken ct)
    {
        var workspace = Domain.Workspace.Create(request.Name, request.Description, request.OwnerId);

        await _workspaceRepo.AddAsync(workspace, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success(new WorkspaceResponse(
            workspace.Id,
            workspace.Name,
            workspace.Description,
            workspace.OwnerId,
            workspace.CreatedAt));
    }
}
