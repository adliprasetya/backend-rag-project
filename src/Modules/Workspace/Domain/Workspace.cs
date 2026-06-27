using SharedKernel.Domain;

namespace Workspace.Domain;

public class Workspace : BaseEntity
{
    private readonly List<WorkspaceMember> _members = [];

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid OwnerId { get; private set; }
    public IReadOnlyCollection<WorkspaceMember> Members => _members.AsReadOnly();

    private Workspace() { }

    public static Workspace Create(string name, string? description, Guid ownerId)
    {
        var workspace = new Workspace
        {
            Name = name,
            Description = description,
            OwnerId = ownerId,
        };
        workspace._members.Add(WorkspaceMember.Create(ownerId, WorkspaceRole.Owner));
        return workspace;
    }

    public void UpdateDetails(string name, string? description)
    {
        Name = name;
        Description = description;
        Touch();
    }

    public void AddMember(Guid userId, WorkspaceRole role)
    {
        if (_members.Any(m => m.UserId == userId))
            return;

        _members.Add(WorkspaceMember.Create(userId, role));
        Touch();
    }

    public void RemoveMember(Guid userId)
    {
        if (userId == OwnerId)
            return;

        _members.RemoveAll(m => m.UserId == userId);
        Touch();
    }

    public bool IsMember(Guid userId) =>
        _members.Any(m => m.UserId == userId);
}
