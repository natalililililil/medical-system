using AuthService.Infrastructure.Persistence;
using MedicalSystem.Shared.Outbox;
using MedicalSystem.Shared.Outbox.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AuthService.Infrastructure.Outbox;

public class AuthOutboxWorker(IServiceScopeFactory _scopeFactory, IMessageBroker _producer, ILogger<AuthOutboxWorker> _logger) 
    : BaseOutboxWorker<AuthDbContext>(_scopeFactory, _producer, _logger) { }