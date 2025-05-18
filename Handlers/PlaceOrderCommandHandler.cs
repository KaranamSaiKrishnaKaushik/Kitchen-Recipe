using Commands;
using Data;
using DataModels;
using MediatR;

namespace Handlers;

public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand,  OrderHistory?>
{
    private readonly DataContext _context;

    public PlaceOrderCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task< OrderHistory?> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = GenerateOrderId();
        var totalPrice = request.Items.Sum(i => i.Price * i.Quantity);

        var orderedItems = request.Items.Select(item => new OrderedItem
        {
            AuthenticationUid = request.AuthenticationUid,
            OrderId = orderId,
            Quantity = item.Quantity,
            Name = item.Name,
            Price = item.Price,
            Currency = item.Currency,
            ImageLink = item.ImageLink,
            ProductId = item.ProductId,
            Category = item.Category,
            IsOnSale = item.IsOnSale,
            SourceName = item.sourceName
        }).ToList();

        await _context.OrderedItems.AddRangeAsync(orderedItems, cancellationToken);

        var orderHistory = new OrderHistory
        {
            AuthenticationUid = request.AuthenticationUid,
            OrderId = orderId,
            TotalOrderPrice = totalPrice,
            IsPaymentCompleted = true
        };

        _context.OrderHistories.Add(orderHistory);

        var userCartItems = _context.ShoppingCart.Where(c => c.AuthenticationUid == request.AuthenticationUid);
        _context.ShoppingCart.RemoveRange(userCartItems);

        var success = await _context.SaveChangesAsync(cancellationToken) > 0;

        return success ? orderHistory : null;
    }

    private string GenerateOrderId()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 10)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());
    }
}
