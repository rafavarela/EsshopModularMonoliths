namespace Shared.Messaging.Events;

public record IntegrationEvent
{
    public Guid EvntId => Guid.NewGuid();
    public DateTime OccurredOn => DateTime.UtcNow;
    //public string EventType => GetType().AssemblyQualifiedName ?? string.Empty;
    public string EventType => GetType().AssemblyQualifiedName;
}
