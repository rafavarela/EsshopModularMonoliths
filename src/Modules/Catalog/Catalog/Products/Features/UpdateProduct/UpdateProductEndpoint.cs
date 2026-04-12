namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductRequest(ProductDto Product);
public record UpdateProductReesponse(bool IsSuccess);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request,
            ISender sender) =>
        {
            var command = request.Adapt<UpdateProductCommand>();

            var result = await sender.Send(command);
            
            var response = result.Adapt<UpdateProductReesponse>();

            return Results.Ok(response);
        })
        .WithName("UpdateProduct")
        .Produces<UpdateProductReesponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Updates an existing product")
        .WithDescription("Updates an existing product with the provided information.");
    }
}
