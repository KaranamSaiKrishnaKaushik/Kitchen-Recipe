using System.Text.Json;
using Commands;
using Data;
using DataModels;
using DTOs;
using Handlers;
using Mappings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Queries;
using Microsoft.AspNetCore.Authorization;

namespace Kitchen_Recipe;

public static class MapEndpointsExtension
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("/",() => 
            new { message = "This is protected data!" });
        
        app.MapPost("/add-recipe", async (
            [FromBody] AddRecipeCommand command,
            [FromServices] AddRecipeCommandHandler handler) =>
        {
            var recipe = await handler.HandleAddRecipe(command);
            return Results.Created($"/add-recipe/{recipe.Id}", recipe.Id);
        });

        app.MapPost("/add-all-recipes",async (
            [FromBody] List<AddRecipeCommand> commands,
            [FromServices] AddRecipeCommandHandler handler) =>
        {
            var recipes = await handler.HandleAddRecipes(commands);
            return Results.Created("/add-all-recipes", recipes.Select(r => r.Id));
        });

        app.MapGet("/fetch-all-recipes", async (
            [FromServices] DataContext context) =>
        {
            var recipes = await context.Recipe
                .Include(r => r.Ingredients)
                .ThenInclude(i => i.BaseName)
                .Select(r => new
                {
                    Id=r.Id,
                    name = r.Name,
                    description = r.Description,
                    imagePath = r.ImagePath,
                    category = r.Category,
                    instructions = r.Instructions,
                    CreatedDate = r.CreatedDate,
                    UpdatedDate = r.UpdatedDate,
                    ingredients = r.Ingredients.Select(i => new
                    {
                        amount = i.Amount,
                        baseName = new
                        {
                            name = i.BaseName.Name
                        }
                    }).ToList()
                })
                .ToListAsync();

            return Results.Ok(recipes);
        });

        app.MapPost("/add-single-ingredient", async (
            [FromBody] AddIngredientCommand command,
            [FromServices] AddIngredientCommandHandler handler) =>
        {
            var result = await handler.HandleAddIngredient(command);
            return Results.Ok(result);
        });

        app.MapPost("/add-multiple-ingredients", async (
            [FromBody] List<AddIngredientCommand> command,
            [FromServices] AddIngredientCommandHandler handler) =>
        {
            var result = await handler.HandleAddMultipleIngredients(command);
            return Results.Ok(result);
        });

        app.MapPost("/update-single-ingredient", async (
            [FromBody] AddIngredientCommand command,
            [FromServices] AddIngredientCommandHandler handler) =>
        {
            var result = await handler.HandleUpdateIngredient(command);
            return Results.Ok(result);
        });
        
        app.MapDelete("/remove-ingredient", async (
            [FromBody] AddIngredientCommand command,
            [FromServices] AddIngredientCommandHandler handler) =>
        {
            var result = await handler.HandleRemoveIngredient(command);
            return Results.Ok(result);
        });

        app.MapGet("/fetch-all-ingredients", async ([FromServices] DataContext dataContext) =>
        {
            var ingredients = await dataContext.Ingredient
                .Include(i => i.BaseName)
                .Select(i => new
                {
                    Id=i.Id,
                    BaseName = new
                    {
                        Name = i.BaseName.Name
                    },
                    Amount = i.Amount
                })
                .ToListAsync();
            return Results.Ok(ingredients);
        });

        app.MapGet("/get-single-ingredient/{name}", async (
            string name,
            [FromServices] DataContext dataContext) =>
        {
            var ingredient = await dataContext.Ingredient
                .Include(r => r.BaseName)
                .FirstOrDefaultAsync(r => r.BaseName.Name == name);

            if (ingredient == null)
                return Results.NotFound("Ingredient not found");

            return Results.Ok(ingredient);
        });

        app.MapGet("/ingredient/search", async (string query, [FromServices] DataContext db) =>
        {
            var results = await db.IngredientBase
                .Where(i => i.Name.Contains(query))
                .ToListAsync();
            return Results.Ok(results);
        });
        
        /* Paypal services start */
        app.MapPost("/api/paypal/create-order", async (PayPalCreateOrderRequestDto body, PayPalService paypal) =>
        {
            string value = body?.Value ?? "1.00";
            var orderId = await paypal.CreateOrder(value);
            return Results.Ok(new { id = orderId });
        });

        app.MapPost("/api/paypal/capture-order-1", async (PayPalCaptureOrderRequestDto  body, PayPalService paypal) =>
        {
            string orderId = body?.OrderId;
            var result = await paypal.CaptureOrder(orderId);
            return Results.Ok(result);
        });
        
        app.MapPost("/api/paypal/capture-order", async (PayPalCaptureOrderRequestDto body, PayPalService paypal) =>
        {
            string orderId = body?.OrderId;
            var rawJson = await paypal.CaptureOrderRaw(orderId);
            Console.WriteLine("PayPal Capture Raw JSON:");
            Console.WriteLine(rawJson);
            return Results.Text(rawJson, "application/json");
        });
        /* Paypal services end*/
        
        /* Products API */
        app.MapPost("/api/products/amazon", async (AmazonProductRoot input, AddProductsCommandHandler handler) =>
        {
            var products = input.Products.Select(ProductMapper.FromAmazon).ToList();
            await handler.Handle(products);
            return Results.Ok(products);
        });

        app.MapPost("/api/products/walmart", async (List<WalmartProductDto> input, AddProductsCommandHandler handler) =>
        {
            var products = input.Select(ProductMapper.FromWalmart).ToList();
            await handler.Handle(products);
            return Results.Ok(products);
        });

        app.MapPost("/api/products/rewe", async (List<ReweProductDto> input, AddProductsCommandHandler handler) =>
        {
            var products = input.Select(ProductMapper.FromRewe).ToList();
            await handler.Handle(products);
            return Results.Ok(products);
        });

        // GET by source
        app.MapGet("/api/products/{source}", async (string source, GetProductsQueryHandler handler) =>
        {
            var products = await handler.Handle(source);
            return Results.Ok(products);
        }); 
        
        app.MapPost("/api/products/searchByNames", async (ProductSearchRequest request, GetProductsQueryHandler handler) =>
        {
            var result = await handler.SearchByNames(request.Source, request.Names);
            return Results.Ok(result);
        });
    }
}