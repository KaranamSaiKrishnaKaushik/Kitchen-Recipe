namespace DataModels;

public class AddIngredientDto
{
    public IngredientBase BaseName { get; set; } = default!;
    public int Amount { get; set; }
}