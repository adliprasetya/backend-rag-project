namespace Workspace.Domain;

public enum WorkspaceRole
{
    Owner,
    Admin,
    Member,
    Viewer,
}

public class WorkspaceMember
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; private set; }
    public WorkspaceRole Role { get; private set; }

    private WorkspaceMember() { }

    public static WorkspaceMember Create(Guid userId, WorkspaceRole role) =>
        new() { UserId = userId, Role = role };
}
