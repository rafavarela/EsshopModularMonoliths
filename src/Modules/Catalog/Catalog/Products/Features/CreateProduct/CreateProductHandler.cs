namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand(ProductDto Product) 
    : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductHandler(CatalogDBContext dbContext) :
    ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, 
        CancellationToken cancellationToken)
    {
        // Create product entity from command object
        var product = CreateNewProduct(command.Product);

        // Save product entity to database
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        // Return result with new product ID
        return new CreateProductResult(product.Id);
    }

    private Product CreateNewProduct(ProductDto product)
    {
        var newProduct = Product.Create(
            Guid.NewGuid(),
            product.Name,
            product.Category,
            product.Description,
            product.ImageFile,
            product.Price
        );

        return newProduct;
    }
}
