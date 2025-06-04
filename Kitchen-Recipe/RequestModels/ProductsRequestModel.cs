using Commands;
using Data;
using DTOs;
using Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Queries;

namespace Kitchen_Recipe.RequestDTOs;

public class AmazonProductsRequest
{
    [FromBody]
    public AmazonProductRoot Input { get; set; }

    [FromServices]
    public AddProductsCommandHandler Handler { get; set; }
}


public class WalmartProductsRequest
{
    [FromBody]
    public List<WalmartProductDto> Input { get; set; }

    [FromServices]
    public AddProductsCommandHandler Handler {  get; set; }
}

public class AllStoresProductsRequest
{
    [FromBody]
    public List<AllStoresProductsDto> Input { get; set; }
    
    [FromServices]
    public AddProductsCommandHandler Handler {  get; set; }
}

public class GetPagedProductsRequest
{
    [FromRoute]
    public string Source { get; set; }

    [FromQuery]
    public int Page { get; set; }

    [FromQuery]
    public int PageSize { get; set; }

    [FromServices]
    public GetProductsQueryHandler Handler { get; set; }
}

public class TranslateRequest
{
    public string Text { get; set; } = string.Empty;
}

public class TranslateTextRequest
{
    [FromBody]
    public required TranslateRequest Request { get; set; }

    [FromServices]
    public required HttpClient HttpClient { get; set; }

    [FromServices]
    public required IOptions<AzureTranslatorOptions> Options { get; set; }
}

public class ProductSearchNames
{
    [FromBody]
    public required ProductSearchRequest Request { get; set; }

    [FromServices]
    public required GetProductsQueryHandler Handler { get; set; }
}

public class GetStoreProducts
{
    [FromRoute]
    public required string Name { get; set; }

    [FromServices]
    public required GetProductsQueryHandler Handler { get; set; }
}

public class AddToShoppingCart
{
    [FromBody]
    public required AddShoppingCartCommand Command { get; set; }

    [FromServices]
    public required AddToShoppingCartCommandHandler Handler { get; set; }

    [FromServices]
    public required IHttpContextAccessor HttpContextAccessor { get; set; }
}


public class RemoveFromCart
{
    [FromRoute]
    public required string ProductId { get; set; }

    [FromServices]
    public required DataContext Context { get; set; }

    [FromServices]
    public required IHttpContextAccessor HttpContextAccessor { get; set; }
}

public class AddCartBulk
{
    [FromBody]
    public required List<AddShoppingCartCommand> Items { get; set; }

    [FromServices]
    public required AddCartBulkHandler Handler { get; set; }

    [FromServices]
    public required IHttpContextAccessor HttpContextAccessor { get; set; }
}

public class ShoppingCartList
{
    [FromServices]
    public required GetShoppingCartQueryHandler Handler { get; set; }

    [FromServices]
    public required IHttpContextAccessor HttpContextAccessor { get; set; }
}
