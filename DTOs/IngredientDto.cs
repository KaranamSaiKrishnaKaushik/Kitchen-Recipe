using DataModels;

namespace DTOs;

public class IngredientDto
{
    public IngredientBaseDto BaseName { get; set; } = default!;
    public int Amount { get; set; }
}

public class IngredientBaseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}