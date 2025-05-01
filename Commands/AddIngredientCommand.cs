using DataModels;

namespace Commands;

public class AddIngredientCommand
{
    public IngredientBase BaseName { get; set; }
    public int Amount { get; set; }
    public string? AuthenticationUid { get; set; }

    public Ingredient ToEntity()
    {
        return new Ingredient
        {
            BaseName = BaseName,
            Amount = Amount,
            AuthenticationUid = AuthenticationUid
        };
    }
}