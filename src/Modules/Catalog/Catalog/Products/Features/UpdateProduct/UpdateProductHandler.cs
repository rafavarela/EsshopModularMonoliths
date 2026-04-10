namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand(ProductDto Product) 
    : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductHandler(CatalogDBContext dbContext)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, 
        CancellationToken cancellationToken)
    {
        // Validate if the product exists in the database
        var product = await dbContext.Products
            .FindAsync([command.Product.Id], cancellationToken);

        if (product is null) 
        {
            throw new Exception($"Product not found: {command.Product.Id}");
        }

        // Update product entity from command object
        UpdateProductWithNewValues(product, command.Product);

        // Save changes to the database
        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Return result indicating success or failure
        return new UpdateProductResult(IsSuccess: true);
    }

    private void UpdateProductWithNewValues(Product product, ProductDto productDto)
    {
        product.Update(
            productDto.Name,
            productDto.Category,
            productDto.Description,
            productDto.ImageFile,
            productDto.Price
        );
    }
}
