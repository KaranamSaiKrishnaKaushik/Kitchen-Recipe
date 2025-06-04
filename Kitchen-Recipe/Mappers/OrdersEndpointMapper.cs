using Commands;
using Kitchen_Recipe.RequestDTOs;
using Microsoft.EntityFrameworkCore;

namespace Kitchen_Recipe;

public class OrdersEndpointMapper : IEndpointMapper
{
     public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders");
        group.MapPost("place-order", async (
            [AsParameters] PlaceOrderRequest req) =>
        {
            var authenticationUid = req.HttpContextAccessor.HttpContext?.GetUserId();
            if (string.IsNullOrWhiteSpace(authenticationUid)) return Results.Unauthorized();

            var command = new PlaceOrderCommand
            {
                AuthenticationUid = authenticationUid,
                Items = req.Items,
            };

            var orderHistory = await req.Mediator.Send(command);

            return orderHistory != null
                ? Results.Ok(orderHistory)
                : Results.BadRequest("Failed to place order");
        }).RequireAuthorization();

        group.MapGet("history", async (
            [AsParameters] ItemHistoryRequest req) =>
        {
            var authenticationUid = req.HttpContextAccessor.HttpContext?.GetUserId();
            if (string.IsNullOrWhiteSpace(authenticationUid)) return Results.Unauthorized();

            var history = await req.Context.OrderHistories
                .Where(o => o.AuthenticationUid == authenticationUid)
                .OrderByDescending(o => o.CreatedDateTime)
                .ToListAsync();
            return Results.Ok(history);
        }).RequireAuthorization();

        group.MapGet("items", async (
            [AsParameters] ItemHistoryRequest req) =>
        {
            var authenticationUid = req.HttpContextAccessor.HttpContext?.GetUserId();
            if (string.IsNullOrWhiteSpace(authenticationUid)) return Results.Unauthorized();

            var orderedItems = await req.Context.OrderedItems
                .Where(o => o.AuthenticationUid == authenticationUid)
                .OrderByDescending(o => o.OrderId)
                .ToListAsync();

            return Results.Ok(orderedItems);
        }).RequireAuthorization();
        
    }  
}