using Basket.Basket.Features.UpdateItemPriceInBasket;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;

namespace Basket.Basket.EventHandlers;

public class ProductPriceChangedIntegrationEventHandler(ISender sender,
    ILogger<ProductPriceChangedIntegrationEventHandler> logger)
    : IConsumer<ProductPriceChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        var message = context.Message;
        // Handle the integration event here
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", message.GetType().Name);

        // MediatR new command and handler to find products in basket and update price
        var command = new UpdateItemPriceInBasketCommand(message.ProductId, message.Price);
        var result = await sender.Send(command);

        if (!result.IsSuccess)
        {
            logger.LogError("Error updating item price in basket for ProductId: {ProductId}", message.ProductId);
        }

        logger.LogInformation("Price for product id: {ProductId} updated in basket", message.ProductId);
    }
}
