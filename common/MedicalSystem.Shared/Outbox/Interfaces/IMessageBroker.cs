namespace MedicalSystem.Shared.Outbox.Interfaces;

public interface IMessageBroker
{
    Task PublishAsync(string topic, string message, CancellationToken cancellationToken);
}