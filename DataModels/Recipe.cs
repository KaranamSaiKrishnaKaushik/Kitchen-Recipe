namespace DataModels;

public class Recipe
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string AuthenticationUid { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public List<RecipeIngredients> Ingredients { get; set; } = new();
}