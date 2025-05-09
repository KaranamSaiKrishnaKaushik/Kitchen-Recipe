using Commands;
using Data;
using DTOs;
using Handlers;
using Mappings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Queries;

namespace Kitchen_Recipe;

public static class MapEndpointsExtension
{
    private  const string UserId = "user_id";
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("/",() => 
            new { message = "You may reach the list of APIs by adding '/swagger' to the URL!" });
        
        app.MapGet("/get-user", async (
            HttpContext httpContext,
            IMediator mediator
        ) => {
            var firebaseUid = httpContext.User.FindFirst("user_id")?.Value;
            if (string.IsNullOrWhiteSpace(firebaseUid))
                return Results.Unauthorized();

            var query = new GetUserByAuthUidQuery(firebaseUid);
            var result = await mediator.Send(query);
            return result == null ? Results.NotFound() : Results.Ok(result);
        }).RequireAuthorization();
        
        app.MapPost("/add-user", async (
            HttpContext httpContext,
            [FromBody] UserDto userDto,
            IMediator mediator
            )=>{
            var firebaseUid = httpContext.User.FindFirst(UserId)?.Value;
            //var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // httpContext.User.FindFirst("sub")?.Value; 
            // later for Azure B2C
            if (string.IsNullOrWhiteSpace(firebaseUid))
                return Results.Unauthorized();

            var command = new AddUserCommand(userDto, firebaseUid);
            var result = await mediator.Send(command);
            return Results.Ok(result);
            
        }).RequireAuthorization();
        
        app.MapPut("/update-user", async (
            HttpContext httpContext,
            [FromBody] UserDto userDto,
            IMediator mediator
        ) =>
        {
            var firebaseUid = httpContext.User.FindFirst("user_id")?.Value;
            if (string.IsNullOrWhiteSpace(firebaseUid))
                return Results.Unauthorized();

            var command = new UpdateUserCommand(userDto, firebaseUid);
            var result = await mediator.Send(command);

            return result == null ? Results.NotFound() : Results.Ok(result);
        }).RequireAuthorization();
        
        app.MapPost("/add-recipe", async (
            HttpContext httpContext,
            [FromBody] AddRecipeCommand command,
            [FromServices] AddRecipeCommandHandler handler) =>
        {
            var firebaseUid = httpContext.User.FindFirst(UserId)?.Value;
            command.AuthenticationUid = firebaseUid;
            var recipe = await handler.HandleAddRecipe(command);
            return Results.Created($"/add-recipe/{recipe.Id}", recipe.Id);
        }).RequireAuthorization();

        app.MapPost("/update-recipe", async (
            HttpContext httpContext,
            [FromBody] UpdateRecipeCommand command,
            [FromServices] UpdateRecipeCommandHandler handler) =>
        {
            var firebaseUid = httpContext.User.FindFirst("user_id")?.Value;
            command.AuthenticationUid = firebaseUid;
            var updatedRecipe = await handler.HandleUpdateRecipe(command);
            return Results.Ok(updatedRecipe);
        }).RequireAuthorization();
        
        app.MapDelete("/delete-recipe/{id:guid}", async (
            Guid id,
            HttpContext httpContext,
            DataContext context
        ) =>
        {
            var firebaseUid = httpContext.User.FindFirst(UserId)?.Value;

            if (string.IsNullOrWhiteSpace(firebaseUid))
                return Results.Unauthorized();

            var recipe = await context.Recipe
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == id && r.AuthenticationUid == firebaseUid);

            if (recipe == null)
                return Results.NotFound("Recipe not found");

            context.RecipeIngredients.RemoveRange(recipe.Ingredients);
            context.Recipe.Remove(recipe);

            await context.SaveChangesAsync();
            return Results.Ok("Recipe deleted successfully.");
        }).RequireAuthorization();

        
         
        app.MapPost("/add-all-recipes", async (
            HttpContext httpContext,
            [FromBody] List<AddRecipeCommand> commands,
            [FromServices] AddRecipeCommandHandler handler) =>
        {
            var firebaseUid = httpContext.User.FindFirst(UserId)?.Value;

            if (string.IsNullOrWhiteSpace(firebaseUid))
                return Results.Unauthorized();

            foreach (var cmd in commands)
            {
                cmd.AuthenticationUid = firebaseUid;
            }

            var recipes = await handler.HandleAddAllRecipes(commands);
            return Results.Created("/add-all-recipes", recipes.Select(r => r.Id));
        }).RequireAuthorization();


        app.MapGet("/fetch-all-recipes", async (HttpContext httpContext,
            [FromServices] DataContext context) =>
        {
            var category = httpContext.Request.Query["category"].ToString();
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault();
            Console.WriteLine($"Authorization Header: {token}");

            var user = httpContext.User;
            var firebaseUid = httpContext.User.FindFirst(UserId)?.Value;
            if (user.Identity is { IsAuthenticated: true })
            {
                Console.WriteLine($"User {user.Identity.Name} is authenticated.");
            }

            Console.WriteLine($"Category: {category}");
            Console.WriteLine($"Authorization Header: {token}");
            Console.WriteLine($"Firebase UID: {firebaseUid}");

            var recipes = await context.Recipe
                .Include(r => r.Ingredients)
                .ThenInclude(i => i.BaseName)
                .Where(r => r.AuthenticationUid == firebaseUid)
                .Select(r => new
                {
                    Id = r.Id,
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
        }).RequireAuthorization();
        
        app.MapPost("/add-single-ingredient", async (
            HttpContext httpContext,
            [FromBody] AddIngredientCommand command,
            [FromServices] AddIngredientCommandHandler handler) =>
        {
            var firebaseUid = httpContext.User.FindFirst(UserId)?.Value;
            command.AuthenticationUid = firebaseUid;
            var result = await handler.HandleAddIngredient(command);
            return Results.Ok(result);
        }).RequireAuthorization();

        app.MapPost("/add-multiple-ingredients", async (
            HttpContext httpContext,
            [FromBody] List<AddIngredientCommand> command,
            [FromServices] AddIngredientCommandHandler handler) =>
        {
            var firebaseUid = httpContext.User.FindFirst(UserId)?.Value;
            foreach (var cmd in command)
            {
                cmd.AuthenticationUid = firebaseUid;
            }
            var result = await handler.HandleAddMultipleIngredients(command);
            return Results.Ok(result);
        }).RequireAuthorization();

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

        app.MapGet("/fetch-all-ingredients", async (
            HttpContext httpContext,
            [FromServices] DataContext dataContext) =>
        {
            var firebaseUid = httpContext.User.FindFirst("user_id")?.Value;
            var ingredients = await dataContext.Ingredient
                .Include(i => i.BaseName)
                .Where(r => r.AuthenticationUid == firebaseUid )
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