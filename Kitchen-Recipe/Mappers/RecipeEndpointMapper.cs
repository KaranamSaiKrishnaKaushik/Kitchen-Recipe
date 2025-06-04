using Kitchen_Recipe.RequestDTOs;

namespace Kitchen_Recipe;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class RecipeEndpointMapper : IEndpointMapper
{

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/").RequireAuthorization();

        group.MapPost("add-recipe", async (
            HttpContext httpContext,
            [AsParameters] RecipeRequestModel req) =>
        {
            var authenticationUid = httpContext.GetUserId();
            if (string.IsNullOrWhiteSpace(authenticationUid)) return Results.Unauthorized();

            req.Command.AuthenticationUid = authenticationUid;
            var recipe = await req.Handler.HandleAddRecipe(req.Command);

            return Results.Created($"/add-recipe/{recipe.Id}", recipe.Id);
        });

        group.MapPost("update-recipe", async (
            HttpContext httpContext,
            [AsParameters] UpdateRecipeRequest req) =>
        {
            var authenticationUid = httpContext.GetUserId();
            if (string.IsNullOrWhiteSpace(authenticationUid)) return Results.Unauthorized();

            req.Command.AuthenticationUid = authenticationUid;
            var updatedRecipe = await req.Handler.HandleUpdateRecipe(req.Command);

            return Results.Ok(updatedRecipe);
        });

        group.MapDelete("delete-recipe/{id:guid}", async (
            [AsParameters] DeleteRecipeRequest req) =>
        {
            var authenticationUid = req.HttpContextAccessor.HttpContext?.GetUserId();
            if (string.IsNullOrWhiteSpace(authenticationUid)) return Results.Unauthorized();

            var recipe = await req.Context.Recipe
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == req.Id && r.AuthenticationUid == authenticationUid);

            if (recipe == null)
                return Results.NotFound("Recipe not found");

            req.Context.RecipeIngredients.RemoveRange(recipe.Ingredients);
            req.Context.Recipe.Remove(recipe);
            await req.Context.SaveChangesAsync();

            return Results.Ok("Recipe deleted successfully.");
        });

        group.MapPost("add-all-recipes", async (
            HttpContext httpContext,
            [AsParameters] AddAllRecipeRequest req) =>
        {
            var authenticationUid = httpContext.GetUserId();
            if (string.IsNullOrWhiteSpace(authenticationUid)) return Results.Unauthorized();

            foreach (var cmd in req.Commands)
            {
                cmd.AuthenticationUid = authenticationUid;
            }

            var recipes = await req.Handler.HandleAddAllRecipes(req.Commands);
            return Results.Created("/add-all-recipes", recipes.Select(r => r.Id));
        });

        group.MapGet("fetch-all-recipes", async (
            HttpContext httpContext,
            [AsParameters] FetchAllRecipesRequest req) =>
        {
            var authenticationUid = httpContext.GetUserId();
            if (string.IsNullOrWhiteSpace(authenticationUid)) return Results.Unauthorized();

            var recipes = await req.Context.Recipe
                .Include(r => r.Ingredients)
                .ThenInclude(i => i.BaseName)
                .Where(r => r.AuthenticationUid == authenticationUid)
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                    r.Description,
                    r.ImagePath,
                    r.Category,
                    r.Instructions,
                    r.CreatedDate,
                    r.UpdatedDate,
                    Ingredients = r.Ingredients.Select(i => new
                    {
                        i.Amount,
                        BaseName = new { i.BaseName.Name }
                    }).ToList()
                })
                .ToListAsync();

            return Results.Ok(recipes);
        });
    }
}
