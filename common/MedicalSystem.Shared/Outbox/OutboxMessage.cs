namespace MedicalSystem.Shared.Outbox;

public class OutboxMessage
{
    public Guid Id { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public DateTime OccurredOnUtc { get; private set; }
    public DateTime? ProcessedOnUtc { get; private set; }
    public string? Error { get; private set; }
    private OutboxMessage() { }

    public OutboxMessage(string type, string content)
    {
        Id = Guid.NewGuid();
        Type = type;
        Content = content;
        OccurredOnUtc = DateTime.UtcNow;
    }

    public void MarkAsProcessed()
    {
        ProcessedOnUtc = DateTime.UtcNow;
        Error = null;
    }

    public void SetError(string error)
    {
        Error = error;
    }
}