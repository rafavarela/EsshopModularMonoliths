namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand(ProductDto Product) 
    : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

// Create command validator using FluentValidation
public class UpdateProductCommandValidator : 
    AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Product.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

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
            throw new ProductNotFoundException(command.Product.Id);
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
