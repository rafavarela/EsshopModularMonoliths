namespace Basket.Basket.Features.UpdateItemPriceInBasket;

public record UpdateItemPriceInBasketCommand(Guid ProductId, decimal Price) 
    : ICommand<UpdateItemPriceInBasketResult>;

public record UpdateItemPriceInBasketResult(bool IsSuccess);

public class UpdateItemPriceInBasketCommandValidator 
    : AbstractValidator<UpdateItemPriceInBasketCommand>
{
    public UpdateItemPriceInBasketCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}

internal class UpdateItemPriceInBasketHandler
    (BasketDbContext dbContext)
    : ICommandHandler<UpdateItemPriceInBasketCommand, 
        UpdateItemPriceInBasketResult>
{
    public async Task<UpdateItemPriceInBasketResult> Handle(
        UpdateItemPriceInBasketCommand command, 
        CancellationToken cancellationToken)
    {
        // Find shopping cart items with a given product id
        var itemsToUpdate = await dbContext.ShoppingCartItems
            .Where(i => i.ProductId == command.ProductId)
            .ToListAsync(cancellationToken);
        
        if (!itemsToUpdate.Any())
        {
            return new UpdateItemPriceInBasketResult(IsSuccess: false);
        }
        
        // Iterate items and update price of every item with incoming command.price
        foreach (var item in itemsToUpdate)
        {
            item.UpdatePrice(command.Price);
        }
        
        // Save changes to the database
        await dbContext.SaveChangesAsync(cancellationToken);
        
        // Return result
        return new UpdateItemPriceInBasketResult(IsSuccess: true);
    }
}
