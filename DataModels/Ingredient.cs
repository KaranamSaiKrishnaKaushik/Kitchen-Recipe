namespace DataModels;

public class Ingredient
{
    public Guid Id { get; set; }
    public IngredientBase BaseName { get; set; }
    public int Amount { get; set; }
}