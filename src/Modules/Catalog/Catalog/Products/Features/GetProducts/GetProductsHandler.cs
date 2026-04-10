namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery() : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<ProductDto> Products);

public class GetProductsHandler(CatalogDBContext dbContext)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, 
        CancellationToken cancellationToken)
    {
        // Get products using dbContext
        var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        // Mapping product entities to product DTOs using Mapster
        var productDtos = products.Adapt<List<ProductDto>>();

        // return result
        return new GetProductsResult(productDtos);
    }

}
