using Data;
using DataModels;

namespace Commands;

public class AddProductsCommandHandler
{
    private readonly DataContext _context;

    public AddProductsCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task Handle<T>(List<T> products) where T : ProductBase
    {
        _context.AddRange(products);
        await _context.SaveChangesAsync();
    }
}