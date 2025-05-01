namespace DataModels;

public class RecipeIngredients
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid RecipeId { get; set; }
    public Recipe Recipe { get; set; } = default!;

    public Guid BaseNameId { get; set; }
    public IngredientBase BaseName { get; set; } = default!;
    
    public int Amount { get; set; }
}