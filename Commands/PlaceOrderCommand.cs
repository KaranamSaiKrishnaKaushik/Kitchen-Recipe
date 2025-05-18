using DataModels;
using DTOs;
using MediatR;

namespace Commands;

public class PlaceOrderCommand : IRequest<OrderHistory>
{
    public List<AllStoresProductsWithQuantityDto> Items { get; set; }
    public string AuthenticationUid { get; set; } = string.Empty;
}