namespace Catalog.Products.Features.GetProductById;

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
            throw new ProductNotFoundException(query.Id);
        }

        // Map the product to ProductDto
        var productDto = product.Adapt<ProductDto>();

        // Return the result
        return new GetProductByIdResult(productDto);
    }
}
