namespace DTOs;

public class RecipeDto
{
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public string Description { get; set; }
    public List<IngredientDto> Ingredients { get; set; }
    public string Category { get; set; } 
    public string Instructions { get; set; } 
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}