using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.DDD;

namespace Shared.Data.Interceptors;

public class DispatchDomainEventsInterceptor(IMediator mediator)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEvents(DbContext? context)
    {
        if (context == null) return;

        // 1. Find all tracked aggregates that have pending domain events
        var aggregates = context.ChangeTracker
            .Entries<IAggregate>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents != null && e.DomainEvents.Any());

        // 2. Collect all the events
        var domainEvents = aggregates
            .SelectMany(a => a.DomainEvents!)
            .ToList();

        // 3. Clear events BEFORE dispatching (prevents re-publishing on retry)
        aggregates.ToList().ForEach(a => a.ClearDomainEvents());

        // 4. Publish each event via MediatR
        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
            //Console.WriteLine($"Dispatching domain event: {domainEvent.GetType().Name}");
        }

    }
}
