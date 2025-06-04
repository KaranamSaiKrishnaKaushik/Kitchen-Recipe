using System.Text;
using System.Text.Json;
using Kitchen_Recipe.RequestDTOs;
using Mappings;
using Microsoft.EntityFrameworkCore;

namespace Kitchen_Recipe;

public class ProductsEndpointMapper : IEndpointMapper
{
   public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products");
        
        group.MapPost("amazon", async (
            [AsParameters] AmazonProductsRequest req) =>
        {
            var products = req.Input.Products.Select(ProductMapper.FromAmazon).ToList();
            await req.Handler.Handle(products);
            return Results.Ok(products);
        });

        group.MapPost("walmart", async (
            [AsParameters] WalmartProductsRequest req) =>
        {
            var products = req.Input.Select(ProductMapper.FromWalmart).ToList();
            await req.Handler.Handle(products);
            return Results.Ok(products);
        });

        group.MapPost("all-stores",
            async ( [AsParameters] AllStoresProductsRequest req) =>
            {
                var products = req.Input.Select(ProductMapper.FromAllStoresProducts).ToList();
                await req.Handler.Handle(products);
                return Results.Ok(products);
            });

        group.MapGet("{source}", async (
            [AsParameters] GetPagedProductsRequest req) =>
        {
            if (req.Page <= 0 || req.PageSize <= 0)
                return Results.BadRequest("Page and PageSize must be greater than 0");

            var result = await req.Handler.GetPagedProducts(req.Source, req.Page, req.PageSize);
            return Results.Ok(result);
        });

        group.MapPost("translate", async (
            /*TranslateRequest request,
            HttpClient http,
            IOptions<AzureTranslatorOptions> options*/
            [AsParameters] TranslateTextRequest req
            ) =>
        {
            var key = req.Options.Value.TranslatorApiKey;
            var region = req.Options.Value.TranslatorRegion;
            var endpoint = req.Options.Value.TranslatorEndpoint;

            var body = new[] { new { Text = req.Request.Text } };
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            req.HttpClient.DefaultRequestHeaders.Clear();
            req.HttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
            req.HttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", region);

            var res = await req.HttpClient.PostAsync(endpoint, content);
            var json = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
                return Results.Problem($"Translation failed: {res.StatusCode} - {json}");

            var translations = JsonDocument.Parse(json).RootElement[0].GetProperty("translations");
            var translatedText = translations[0].GetProperty("text").GetString();
            return Results.Ok(new { translatedText });
        });

        group.MapPost("searchByNames",
            async ([AsParameters] ProductSearchNames req) =>
            {
                var result = await req.Handler.SearchByNames(
                    req.Request.Source, req.Request.Names);
                return Results.Ok(result);
            });

        group.MapGet("all-store-products/by-store/{name}",
            async ([AsParameters] GetStoreProducts req) =>
            {
                var products = await req.Handler.GetProductsFromOneStore(req.Name);
                return Results.Ok(products);
            });

        group.MapPost("add-to-shopping-cart", async (
            [AsParameters] AddToShoppingCart req) =>
        {
            var authenticationUid = req.HttpContextAccessor.HttpContext?.GetUserId();
            if (string.IsNullOrWhiteSpace(authenticationUid)) return Results.Unauthorized();
            req.Command.AuthenticationUid = authenticationUid;
            var product = await req.Handler.AddToShoppingCart(req.Command);
            return Results.Created($"/add-to-shopping-cart/{product.Id}", product.Id);
        }).RequireAuthorization();

        group.MapDelete("remove-from-cart/{productId}", async (
            [AsParameters] RemoveFromCart req) =>
        {
            var authenticationUid = req.HttpContextAccessor.HttpContext?.GetUserId();
            if (string.IsNullOrWhiteSpace(authenticationUid)) return Results.Unauthorized();

            var cartItem = await req.Context.ShoppingCart
                .FirstOrDefaultAsync(x => x.ProductId == req.ProductId && x.AuthenticationUid == authenticationUid);

            if (cartItem == null)
            {
                return Results.NotFound();
            }

            req.Context.ShoppingCart.Remove(cartItem);
            await req.Context.SaveChangesAsync();

            return Results.Ok("Item removed from cart.");
        }).RequireAuthorization();

        group.MapPost("add-cart-bulk", async (
            [AsParameters] AddCartBulk req) =>
        {
            var authenticationUid = req.HttpContextAccessor.HttpContext?.GetUserId();
            if (string.IsNullOrWhiteSpace(authenticationUid)) return Results.Unauthorized();
            await req.Handler.HandleAsync(req.Items, authenticationUid);
            return Results.Ok();
        }).RequireAuthorization();

        group.MapGet("shopping-cart-list", async (
            [AsParameters] ShoppingCartList req) =>
        {
            var authenticationUid = req.HttpContextAccessor.HttpContext?.GetUserId();
            if (string.IsNullOrWhiteSpace(authenticationUid)) return Results.Unauthorized();
            if (string.IsNullOrWhiteSpace(authenticationUid))
            {
                return Results.Unauthorized();
            }

            var products = await req.Handler.GetUserCartAsync(authenticationUid);
            return Results.Ok(products);
        }).RequireAuthorization();
    }    
}