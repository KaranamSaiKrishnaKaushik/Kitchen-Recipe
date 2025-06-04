using Kitchen_Recipe.RequestDTOs;

namespace Kitchen_Recipe;

public class PaymentEndpointMapper : IEndpointMapper
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/paypal");

        group.MapPost("/create-order", async (
            [AsParameters] CreateRequest req) =>
        {
            string value = req.Body?.Value ?? "1.00";
            var orderId = await req.Paypal.CreateOrder(value);
            return Results.Ok(new { id = orderId });
        });

        group.MapPost("/capture-order-1", async (
            [AsParameters] CaptureOrderRequest req) =>
        {
            string orderId = req.Body.OrderId;
            var result = await req.Paypal.CaptureOrder(orderId);
            return Results.Ok(result);
        });

        group.MapPost("/capture-order", async (
            [AsParameters] CaptureOrderRequest req) =>
        {
            string orderId = req.Body.OrderId;
            var rawJson = await req.Paypal.CaptureOrderRaw(orderId);
            Console.WriteLine("PayPal Capture Raw JSON:");
            Console.WriteLine(rawJson);
            return Results.Text(rawJson, "application/json");
        });
    }
}