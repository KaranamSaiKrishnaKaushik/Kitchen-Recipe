using DataModels;

namespace Commands;

public class UpdateRecipeCommand
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public List<AddIngredientDto> Ingredients { get; set; } = new();
    public string? AuthenticationUid { get; set; }
}