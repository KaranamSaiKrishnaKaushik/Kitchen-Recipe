using DataModels;

namespace Commands;

public class AddRecipeCommand
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public List<AddIngredientDto> Ingredients { get; set; } = new();
    public string? AuthenticationUid { get; set; }

    public Recipe ToEntity()
    {
        var recipeId = Id ?? Guid.NewGuid();
        return new Recipe
        {
            Id = recipeId,
            Name = Name,
            Description = Description,
            ImagePath = ImagePath,
            Category = Category,
            Instructions = Instructions,
            AuthenticationUid =  AuthenticationUid,
            UpdatedDate = null,
            Ingredients = Ingredients.Select(i => new RecipeIngredients
            {
                Id = Guid.NewGuid(),
                BaseName = i.BaseName,
                Amount = i.Amount,
                RecipeId = recipeId,
            }).ToList()
        };
    }
}