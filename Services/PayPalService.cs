using System.Xml;
using PayPalCheckoutSdk.Orders;
using PayPalCheckoutSdk.Core;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

public class PayPalService
{
    private PayPalHttp.HttpClient _client;

    private string clientId = "Ae9Hcx388JWuvk9PypBO8iteGwm06-jOhZjxpAHktDyobKAslFOwnh6Apy8h15udU60ge9WGQUe9xROD";
    string clientSecret = "EFfmcvwUwSx5tZ3YqKSGL8bNVY5GOG2vwBhXrqjA66f8OuFr7009AKAHFzvEycymanrWSSA9tGoLDDZg";
    public PayPalService()
    {
        var environment = new SandboxEnvironment(clientId, clientSecret);
        _client = new PayPalHttpClient(environment);
    }

    public async Task<string> CreateOrder(string amount)
    {
        var orderRequest = new OrderRequest()
        {
            CheckoutPaymentIntent = "CAPTURE",
            PurchaseUnits = new List<PurchaseUnitRequest> {
                new PurchaseUnitRequest {
                    AmountWithBreakdown = new AmountWithBreakdown {
                        CurrencyCode = "EUR",
                        Value = amount
                    }
                }
            }
        };

        var request = new OrdersCreateRequest();
        request.Prefer("return=representation");
        request.RequestBody(orderRequest);

        var response = await _client.Execute(request);
        var result = response.Result<Order>();
        return result.Id;
    }

    public async Task<Order> CaptureOrder(string orderId)
    {
        var request = new OrdersCaptureRequest(orderId);
        request.RequestBody(new OrderActionRequest());
        var response = await _client.Execute(request);
        return response.Result<Order>();
    }
    
    public async Task<string> CaptureOrderRaw(string orderId)
    {
        var request = new OrdersCaptureRequest(orderId);
        request.RequestBody(new OrderActionRequest());
        
        var response = await _client.Execute(request);
        var result = response.Result<Order>();

        var rawJson = JsonConvert.SerializeObject(result, Formatting.Indented);
        return rawJson;
    }
}