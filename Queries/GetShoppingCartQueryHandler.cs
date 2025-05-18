using Data;
using DTOs;
using Microsoft.EntityFrameworkCore;

namespace Queries;

public class GetShoppingCartQueryHandler
{
    private readonly DataContext _context;

    public GetShoppingCartQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<List<AllStoresProductsWithQuantityDto>> GetUserCartAsync(string authUid)
    {
        var cartItems = await _context.ShoppingCart
            .Where(c => c.AuthenticationUid == authUid)
            .ToListAsync();

        var productIds = cartItems
            .Where(c => c.AuthenticationUid == authUid)
            .Select(c => c.ProductId)
            .ToList();

        var products = await _context.AllStoresProducts
            .Where(p => productIds.Contains(p.ProductId))
            .ToListAsync();

        var result = from product in products
            join cart in cartItems on product.ProductId equals cart.ProductId
            select new AllStoresProductsWithQuantityDto
            {
                Name = product.Name,
                Price = product.Price,
                Currency = product.Currency,
                ReviewCount = product.ReviewCount,
                ImageLink = product.ImageLink,
                ProductId = product.ProductId,
                Grammage = product.Grammage,
                Category = product.Category,
                IsOnSale = product.IsOnSale,
                sourceName = product.sourceName,
                Quantity = cart.Quantity
            };

        return result.ToList();
    }
}
