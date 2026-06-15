using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MedicalSystem.Shared.MessageBroker.Kafka;

public abstract class BaseKafkaConsumer<TEvent>(IServiceScopeFactory _scopeFactory, ILogger _logger, string _topic, string _groupId,
    string _bootstrapServers = "localhost:9092") : BackgroundService
{
    protected async override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = _groupId,
            BootstrapServers = _bootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(_topic);

        _logger.LogInformation("Started Kafka consumer for topic: {Topic}", _topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(cancellationToken);

                using var scope = _scopeFactory.CreateScope();

                var data = JsonSerializer.Deserialize<TEvent>(consumeResult.Message.Value);

                if (data != null)
                {
                    using var scopeProvider = _scopeFactory.CreateScope();
                    await ProcessMessageAsync(data, scopeProvider.ServiceProvider, cancellationToken);
                }

                consumer.Commit(consumeResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming message from Kafka");
            }
        }
    }

    protected abstract Task ProcessMessageAsync(TEvent data, IServiceProvider serviceProvider, CancellationToken cancellationToken);
}