namespace Catalog.Products.EventHandlers;

public class ProductCreatedEventHandler
    (ILogger<ProductPriceChangedEventHandler> logger)
    : INotificationHandler<ProductPriceChangedEvent>
{
    public Task Handle(ProductPriceChangedEvent notification, 
        CancellationToken cancellationToken)
    {
        // TODO: Publish product price changed integration event for update basket prices

        logger.LogInformation("Domain event handled: {DomainEvent}",
            notification.GetType().Name);
        
        return Task.CompletedTask;
    }
}
