using MediatR;

namespace SharedKernel.Domain;

public interface IDomainEvent : INotification
{
    DateTime OccurredAt { get; }
}

public abstract class DomainEvent : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
