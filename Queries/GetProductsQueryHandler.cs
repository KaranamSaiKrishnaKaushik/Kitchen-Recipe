using Data;
using DataModels;
using Microsoft.EntityFrameworkCore;

namespace Queries;

public class GetProductsQueryHandler
{
    private readonly DataContext _context;

    public GetProductsQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<List<ProductBase>> Handle(string source)
    {
        return source.ToLower() switch
        {
            "amazon" => await _context.AmazonProducts.ToListAsync<ProductBase>(),
            "walmart" => await _context.WalmartProducts.ToListAsync<ProductBase>(),
            "rewe" => await _context.ReweProducts.ToListAsync<ProductBase>(),
            _ => throw new ArgumentException("Unknown source")
        };
    }
    
    public async Task<List<ProductBase>> SearchByNames(string source, List<string> names)
    {
        var nameFilters = names.Select(n => n.ToLower()).ToList();

        switch (source.ToLower())
        {
            case "amazon":
                return await _context.AmazonProducts
                    .Where(p => nameFilters.Any(n => p.Name.ToLower().Contains(n)))
                    .ToListAsync<ProductBase>();

            case "walmart":
                return await _context.WalmartProducts
                    .Where(p => nameFilters.Any(n => p.Name.ToLower().Contains(n)))
                    .ToListAsync<ProductBase>();

            case "rewe":
                return await _context.ReweProducts
                    .Where(p => nameFilters.Any(n => p.Name.ToLower().Contains(n)))
                    .ToListAsync<ProductBase>();

            default:
                throw new ArgumentException("Invalid source");
        }
    }
}