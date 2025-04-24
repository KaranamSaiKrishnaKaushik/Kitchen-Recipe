using System.Runtime.InteropServices.Marshalling;
using DataModels;
using DTOs;

namespace Commands;

public class AddRecipeCommand
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public List<AddIngredientDto> Ingredients { get; set; } = new();

    public Recipe ToEntity()
    {
        var recipeId = Guid.NewGuid();
        return new Recipe
        {
            Id = recipeId,
            Name = Name,
            Description = Description,
            ImagePath = ImagePath,
            Category = Category,
            Instructions = Instructions,
            Ingredients = Ingredients.Select(i => new RecipeIngredients
            {
                Id = Guid.NewGuid(),
                BaseName = i.BaseName,
                Amount = i.Amount,
                RecipeId = recipeId
            }).ToList()
        };
    }
}