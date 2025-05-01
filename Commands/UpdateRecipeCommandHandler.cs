using Data;
using DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Commands;

public class UpdateRecipeCommandHandler
{
    private readonly DataContext _context;

    public UpdateRecipeCommandHandler(DataContext context)
    {
        _context = context;
    }

   public async Task<IResult> HandleUpdateRecipe(UpdateRecipeCommand command)
   {
       var recipe = await _context.Recipe
           .Include(r => r.Ingredients)
           .FirstOrDefaultAsync(r => r.Id == command.Id);

       if (recipe == null)
           return Results.NotFound("Recipe not found");
       
       recipe.Name = command.Name;
       recipe.Category = command.Category;
       recipe.ImagePath = command.ImagePath;
       recipe.Description = command.Description;
       recipe.Instructions = command.Instructions;
       recipe.UpdatedDate = DateTime.UtcNow;
       
       _context.RecipeIngredients.RemoveRange(recipe.Ingredients);

       foreach (var ing in command.Ingredients)
       {
           var baseName = await _context.IngredientBase
                              .FirstOrDefaultAsync(b => b.Id == ing.BaseName.Id)
                          ?? await _context.IngredientBase
                              .FirstOrDefaultAsync(b => b.Name.ToLower() == ing.BaseName.Name.ToLower());

           if (baseName == null)
           {
               baseName = new IngredientBase
               {
                   Id = Guid.NewGuid(),
                   Name = ing.BaseName.Name
               };
               await _context.IngredientBase.AddAsync(baseName);
               await _context.SaveChangesAsync();
           }

           var newIngredient = new RecipeIngredients
           {
               Id = Guid.NewGuid(),
               Amount = ing.Amount,
               BaseNameId = baseName.Id,
               RecipeId = recipe.Id
           };

           await _context.RecipeIngredients.AddAsync(newIngredient);
       }

       await _context.SaveChangesAsync();
       return Results.Ok("Recipe updated");
   }
   
}
