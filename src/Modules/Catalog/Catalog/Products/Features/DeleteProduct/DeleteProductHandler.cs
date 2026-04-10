namespace Catalog.Products.Features.DeleteProduct;

public record DeleteProductCommand(Guid ProductId) 
    : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductHandler(CatalogDBContext dbContext)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, 
        CancellationToken cancellationToken)
    {
        // Validate if product exists in the database
        var product = await dbContext.Products
            .FindAsync([command.ProductId], cancellationToken);

        if (product is null)
        {
            throw new Exception($"Product not found: {command.ProductId}");
        }

        // Delete product entity from command object
        dbContext.Products.Remove(product);

        // Save changes to database
        await dbContext.SaveChangesAsync();

        // Return result indicating success or failure
        return new DeleteProductResult(IsSuccess: true);
    }
}
