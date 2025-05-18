using Commands;
using Data;
using DataModels;
using Microsoft.EntityFrameworkCore;

namespace Handlers;

public class AddToShoppingCartCommandHandler
{
    private readonly DataContext _context;

    public AddToShoppingCartCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<ShoppingCart> AddToShoppingCart(AddShoppingCartCommand command)
    {
        var existing = await _context.ShoppingCart
            .FirstOrDefaultAsync(x=>x.ProductId == command.ProductId
                                    && x.AuthenticationUid == command.AuthenticationUid);
        
        if (existing != null)
        {
            existing.Quantity = command.Quantity;
        }
        else
        {
            var newCartItem = new ShoppingCart
            {
                ProductId = command.ProductId,
                Quantity = command.Quantity,
                AuthenticationUid = command.AuthenticationUid
            };
            await _context.ShoppingCart.AddAsync(newCartItem);
        }

        await _context.SaveChangesAsync();
        return existing ?? command.ToEntity();
    }

}