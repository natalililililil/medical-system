using MediatR;
using MedicalSystem.Shared.Contracts.Events;
using MedicalSystem.Shared.MessageBroker.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Profiles.Application.Features.Commands.CreateBaseProfile;
namespace Profiles.Infrastructure.MessageBroker;

public class AccountCreatedConsumer(IServiceScopeFactory _scopeFactory, ILogger<AccountCreatedConsumer> _logger) 
    : BaseKafkaConsumer<AccountCreatedEvent>(_scopeFactory, _logger, "account-created", "profiles-group")
{
    protected async override Task ProcessMessageAsync(AccountCreatedEvent data, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        _logger.LogInformation("Routing AccountCreatedEvent to handler for {AccountId}", data.AccountId);

        await mediator.Send(new CreateBaseProfileCommand(data.AccountId, data.Role));
    }
}