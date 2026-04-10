namespace Catalog.Products.Features.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) 
    : IQuery<GetProductByCategoryResult>;

public record GetProductByCategoryResult(IEnumerable<ProductDto> Products);

public class GetProductByCategoryHandler(CatalogDBContext dbContext)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, 
        CancellationToken cancellationToken)
    {
        // Get products by category using DbContext
        var products = await dbContext.Products
            .AsNoTracking()
            .Where(p => p.Category.Contains(query.Category))
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);    
        
        // Mapping product entity to productdto
        var productDtos = products.Adapt<List<ProductDto>>();

        // return result
        return new GetProductByCategoryResult(productDtos);
    }
}
