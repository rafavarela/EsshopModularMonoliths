using Shared.Pagination;

namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery(PaginationRequest PaginationRequest) 
    : IQuery<GetProductsResult>;

public record GetProductsResult(PaginatedResult<ProductDto> Products);

public class GetProductsHandler(CatalogDBContext dbContext)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, 
        CancellationToken cancellationToken)
    {
        // Get products paginated from database
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;
        var totalCount = await dbContext.Products.LongCountAsync(cancellationToken);

        var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        // Mapping product entities to product DTOs using Mapster
        var productDtos = products.Adapt<List<ProductDto>>();

        // return result
        return new GetProductsResult(
            new PaginatedResult<ProductDto>(
                pageIndex, 
                pageSize,
                totalCount,
                productDtos)
            );
    }

}
