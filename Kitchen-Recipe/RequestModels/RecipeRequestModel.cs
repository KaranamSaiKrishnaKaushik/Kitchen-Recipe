using Commands;
using Data;
using Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Kitchen_Recipe.RequestDTOs;

public class RecipeRequestModel
{
    [FromBody] 
    public AddRecipeCommand Command { get; set; }

    [FromServices]
    public AddRecipeCommandHandler Handler { get; set; }
}

public class UpdateRecipeRequest
{
    [FromBody] 
    public UpdateRecipeCommand Command { get; set; }

    [FromServices]
    public UpdateRecipeCommandHandler Handler { get; set; }
}

public class DeleteRecipeRequest
{
    [FromRoute]
    public Guid Id { get; set; }

    [FromServices]
    public DataContext Context { get; set; }

    [FromServices]
    public IHttpContextAccessor HttpContextAccessor { get; set; }
}

public class AddAllRecipeRequest
{
    [FromBody] 
    public List<AddRecipeCommand> Commands { get; set; }

    [FromServices]
    public AddRecipeCommandHandler Handler { get; set; }
}

public class FetchAllRecipesRequest
{
    [FromServices]
    public DataContext Context { get; set; }
}