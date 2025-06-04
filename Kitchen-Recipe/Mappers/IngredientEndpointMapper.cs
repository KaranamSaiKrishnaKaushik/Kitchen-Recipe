using Kitchen_Recipe.RequestDTOs;

namespace Kitchen_Recipe;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

public class IngredientEndpointMapper : IEndpointMapper
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/").RequireAuthorization();

        group.MapPost("add-single-ingredient", async (
            HttpContext httpContext,
            [AsParameters] AddIngredientRequest req) =>
        {
            var authenticationUid = httpContext.GetUserId();
            if (authenticationUid is null) return Results.Unauthorized();

            req.Command.AuthenticationUid = authenticationUid;
            var result = await req.Handler.HandleAddIngredient(req.Command);
            return Results.Ok(result);
        });

        group.MapPost("add-multiple-ingredients", async (
            HttpContext httpContext,
            [AsParameters] AddMultipleIngredientsRequest req) =>
        {
            var authenticationUid = httpContext.GetUserId();
            if (authenticationUid is null) return Results.Unauthorized();

            foreach (var cmd in req.Commands)
            {
                cmd.AuthenticationUid = authenticationUid;
            }

            var result = await req.Handler.HandleAddMultipleIngredients(req.Commands);
            return Results.Ok(result);
        });

        group.MapPost("update-single-ingredient", async (
            [AsParameters] AddIngredientRequest req) =>
        {
            var result = await req.Handler.HandleUpdateIngredient(req.Command);
            return Results.Ok(result);
        });

        group.MapDelete("remove-ingredient", async (
            [AsParameters] AddIngredientRequest req) =>
        {
            var result = await req.Handler.HandleRemoveIngredient(req.Command);
            return Results.Ok(result);
        });

        group.MapGet("fetch-all-ingredients", async (
            [AsParameters] GetUserIngredientsRequest req) =>
        {
            var authenticationUid = req.HttpContextAccessor.HttpContext?.GetUserId();
            if (authenticationUid is null) return Results.Unauthorized();

            var ingredients = await req.Context.Ingredient
                .Include(i => i.BaseName)
                .Where(r => r.AuthenticationUid == authenticationUid)
                .Select(i => new
                {
                    i.Id,
                    BaseName = new { i.BaseName.Name },
                    i.Amount
                })
                .ToListAsync();

            return Results.Ok(ingredients);
        });

        group.MapGet("get-single-ingredient/{name}", async (
            [AsParameters] GetIngredientByNameRequest req) =>
        {
            var ingredient = await req.Context.Ingredient
                .Include(r => r.BaseName)
                .FirstOrDefaultAsync(r => r.BaseName.Name == req.name);

            return ingredient == null
                ? Results.NotFound("Ingredient not found")
                : Results.Ok(ingredient);
        });

        group.MapGet("search", async (
            [AsParameters] SearchIngredientsRequest req) =>
        {
            var results = await req.Db.IngredientBase
                .Where(i => i.Name.Contains(req.Query))
                .ToListAsync();

            return Results.Ok(results);
        });
    }
}
