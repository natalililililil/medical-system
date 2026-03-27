using Confluent.Kafka;
using MedicalSystem.Shared.Outbox.Interfaces;

namespace MedicalSystem.Shared.MessageBroker.Kafka;

public class BaseKafkaProducer(IProducer<string, string> _producer) : IMessageBroker
{
    public async Task PublishAsync(string topic, string message, CancellationToken cancellationToken)
    {
        var kafkaMessage = new Message<string, string> { Value = message };
        await _producer.ProduceAsync(topic, kafkaMessage, cancellationToken);
    }
}