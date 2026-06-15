using MediatR;
using MedicalSystem.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.Shared.Behaviors;

public class TransactionBehavior<TRequest, TResponse>(IAppDbContext context) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (context is not DbContext dbContext)
            return await next();

        if (dbContext.Database.CurrentTransaction != null)
            return await next();

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var response = await next();

            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return response;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}