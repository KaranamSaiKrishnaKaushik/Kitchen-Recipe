using Commands;
using Data;
using DataModels;
using Microsoft.EntityFrameworkCore;

namespace Handlers;

public class AddCartBulkHandler
{
    private readonly DataContext _context;

    public AddCartBulkHandler(DataContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(List<AddShoppingCartCommand> items, string firebaseUid)
    {
        foreach (var command in items)
        {
            var existing = await _context.ShoppingCart
                .FirstOrDefaultAsync(x =>
                    x.ProductId == command.ProductId &&
                    x.AuthenticationUid == firebaseUid);

            if (existing != null)
            {
                existing.Quantity = command.Quantity;
                existing.UpdatedDate = DateTime.UtcNow;
            }
            else
            {
                await _context.ShoppingCart.AddAsync(new ShoppingCart
                {
                    ProductId = command.ProductId,
                    Quantity = command.Quantity,
                    AuthenticationUid = firebaseUid
                });
            }
        }

        await _context.SaveChangesAsync();
    }
}
