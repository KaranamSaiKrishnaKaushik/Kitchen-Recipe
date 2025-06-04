using Data;
using DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kitchen_Recipe.RequestDTOs;

public class PlaceOrderRequest
{
    [FromBody]
    public List<AllStoresProductsWithQuantityDto> Items { get; set; }
    
    [FromServices]
    public IHttpContextAccessor HttpContextAccessor { get; set; }
    
    [FromServices] 
    public IMediator Mediator { get; set; }
}

public class ItemHistoryRequest
{
    [FromServices]
    public IHttpContextAccessor HttpContextAccessor { get; set; }
    
    [FromServices]
    public DataContext Context { get; set; }
}