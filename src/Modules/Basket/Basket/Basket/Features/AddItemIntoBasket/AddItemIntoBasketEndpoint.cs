namespace Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketRequest(string UserName, ShoppingCartItemDto ShoppingCartItem);
public record AddItemIntoBasketresponse(Guid Id);

public class AddItemIntoBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/{userName}/items", 
            async ([FromRoute] string userName, 
            [FromBody] AddItemIntoBasketRequest request,
            ISender sender) =>
        {
            var command = new AddItemIntoBasketCommand(userName, request.ShoppingCartItem);
            var result = await sender.Send(command);
            var response = result.Adapt<AddItemIntoBasketresponse>();
            
            return Results.Created($"/basket/{response.Id}", response);
        })
        .Produces<AddItemIntoBasketresponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Add an item to a shopping basket")
        .WithDescription("Adds an item to the shopping basket for the specified user.")
        .RequireAuthorization();
    }
}
