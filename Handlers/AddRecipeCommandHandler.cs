using Commands;
using Data;
using DataModels;
using Microsoft.EntityFrameworkCore;

namespace Handlers;

public class AddRecipeCommandHandler
{
    private readonly DataContext _context;

    public AddRecipeCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Recipe> HandleAddRecipe(AddRecipeCommand command)
    {
        foreach (var ingredient in command.Ingredients)
        {
            var existingBase = await _context.IngredientBase
                .FirstOrDefaultAsync(b => b.Name == ingredient.BaseName.Name);

            if (existingBase != null)
            {
                ingredient.BaseName = existingBase;
            }
            else
            {
                ingredient.BaseName.Id = Guid.NewGuid();
                _context.IngredientBase.Add(ingredient.BaseName);
            }
        }

        var recipe = command.ToEntity();
        _context.Recipe.Add(recipe);
        await _context.SaveChangesAsync();
        return recipe;
    }

    public async Task<List<Recipe>> HandleAddRecipes(List<AddRecipeCommand> commands)
    {
        foreach (var command in commands)
        {
            foreach (var ingredient in command.Ingredients)
            {
                var existingBase = await _context.IngredientBase
                    .FirstOrDefaultAsync(b => b.Name == ingredient.BaseName.Name);

                if (existingBase != null)
                {
                    ingredient.BaseName = existingBase;
                }
                else
                {
                    ingredient.BaseName.Id = Guid.NewGuid();
                    _context.IngredientBase.Add(ingredient.BaseName);
                }
            }
        }

        var recipes = commands.Select(c => c.ToEntity()).ToList();
        _context.Recipe.AddRange(recipes);
        await _context.SaveChangesAsync();
        return recipes;
    }
}