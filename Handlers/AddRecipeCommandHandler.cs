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
        var existing = await _context.Recipe
            .FirstOrDefaultAsync(r => r.Name.ToLower() == command.Name.ToLower()
                                      && r.AuthenticationUid == command.AuthenticationUid);

        if (existing != null)
        {
            throw new InvalidOperationException("A recipe with the same name already exists.");
        }
        
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

    public async Task<List<Recipe>> HandleAddAllRecipes(List<AddRecipeCommand> commands)
    {
        var allIngredientNames = commands
            .SelectMany(cmd => cmd.Ingredients)
            .Select(ing => ing.BaseName.Name.ToLower())
            .Distinct()
            .ToList();

        var existingBases = await _context.IngredientBase
            .Where(b => allIngredientNames.Contains(b.Name.ToLower()))
            .ToListAsync();
        
        var incomingRecipeKeys = commands
            .Select(c => new { Name = c.Name.ToLower(), Uid = c.AuthenticationUid })
            .ToList();
        
        var existingRecipeKeys = await _context.Recipe
            .Where(r => incomingRecipeKeys.Select(k => k.Uid).Contains(r.AuthenticationUid))
            .ToListAsync();
        
        var filteredCommands = commands
            .Where(cmd => !existingRecipeKeys
                .Any(r => r.Name.ToLower() == cmd.Name.ToLower() && r.AuthenticationUid == cmd.AuthenticationUid))
            .ToList();
        
        foreach (var command in filteredCommands)
        {
            foreach (var ingredient in command.Ingredients)
            {
                var match = existingBases
                    .FirstOrDefault(b => b.Name.Equals(ingredient.BaseName.Name, StringComparison.OrdinalIgnoreCase));

                if (match != null)
                {
                    ingredient.BaseName = match;
                }
                else
                {
                    ingredient.BaseName.Id = Guid.NewGuid();
                    _context.IngredientBase.Add(ingredient.BaseName);
                    existingBases.Add(ingredient.BaseName);
                }
            }
        }

        var recipes = filteredCommands.Select(cmd => cmd.ToEntity()).ToList();
        _context.Recipe.AddRange(recipes);
        await _context.SaveChangesAsync();
        return recipes;
    }
}