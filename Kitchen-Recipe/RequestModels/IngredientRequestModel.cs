using Commands;
using Data;
using Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Kitchen_Recipe.RequestDTOs;

public class AddIngredientRequest
{
    [FromBody] 
    public AddIngredientCommand Command { get; set; }

    [FromServices] 
    public AddIngredientCommandHandler Handler { get; set; }
}

public class AddMultipleIngredientsRequest
{
    [FromBody] 
    public List<AddIngredientCommand> Commands { get; set; }

    [FromServices] 
    public AddIngredientCommandHandler Handler { get; set; }
}

public class GetUserIngredientsRequest
{
    [FromServices]
    public DataContext Context { get; set; }

    [FromServices]
    public IHttpContextAccessor HttpContextAccessor { get; set; }
}

public class GetIngredientByNameRequest
{
    [FromRoute]
    public String name { get; set; }
    
    [FromServices]
    public IHttpContextAccessor HttpContextAccessor { get; set; }
    
    [FromServices]
    public DataContext Context { get; set; }
}

public class SearchIngredientsRequest
{
    [FromQuery]
    public string Query { get; set; }

    [FromServices]
    public DataContext Db { get; set; }
}
