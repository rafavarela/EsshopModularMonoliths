namespace Catalog.Products.Features.GetProductById;

public record GetProductByIdQuery(Guid Id) 
    : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(ProductDto Product);

public class GetProductByIdHandler(CatalogDBContext dbContext)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        //  Get product by id using dbContext
        var product = await dbContext.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

        if (product == null)
        { 
            throw new Exception($"Product with id {query.Id} not found.");
        }

        // Map the product to ProductDto
        var productDto = product.Adapt<ProductDto>();

        // Return the result
        return new GetProductByIdResult(productDto);
    }
}
