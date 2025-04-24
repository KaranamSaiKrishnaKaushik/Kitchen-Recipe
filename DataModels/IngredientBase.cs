namespace DataModels;

public class IngredientBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;

    public List<RecipeIngredients> Recipes { get; set; } = new();
}