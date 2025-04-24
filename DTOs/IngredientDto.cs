using DataModels;

namespace DTOs;

public class IngredientDto
{
    public IngredientBase BaseName { get; set; } = default!;
    //public string Name { get; set; }
    public int Amount { get; set; }
}