using MedicalSystem.Shared.Outbox.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MedicalSystem.Shared.Outbox;

public class BaseOutboxWorker<TContext>(IServiceScopeFactory _scopeFactory, IMessageBroker _producer, ILogger _logger) : BackgroundService
    where TContext : DbContext, IHasOutbox
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Outbox background service is starting");

        while (!cancellationToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();

            var messages = await context.OutboxMessages
                .Where(m => m.ProcessedOnUtc == null)
                .OrderBy(m => m.OccurredOnUtc)
                .Take(20)
                .ToListAsync(cancellationToken);

            if (messages.Count > 0)
            {
                using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    foreach (var message in messages)
                    {
                        try
                        {
                            _logger.LogDebug("Publishing message {Id} of type {Type} to broker", message.Id, message.Type);

                            await _producer.PublishAsync(message.Type, message.Content, cancellationToken);

                            message.MarkAsProcessed();

                            _logger.LogInformation("Successfully published outbox message {Id}", message.Id);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to publish outbox message {Id}", message.Id);
                            message.SetError(ex.Message);
                        }
                    }

                    await context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    _logger.LogError(ex, "Transaction failed, rolling back");
                }
            }
                
            await Task.Delay(10000, cancellationToken);
        }
    }
}
