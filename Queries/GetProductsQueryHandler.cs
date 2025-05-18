using Data;
using DataModels;
using DTOs;
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
            "all-stores" => await _context.AllStoresProducts.ToListAsync<ProductBase>(),
            _ => throw new ArgumentException("Unknown source")
        };
    }
    
    public async Task<PagedResult<ProductBase>> GetPagedProducts(string source, int page, int pageSize)
    {
        IQueryable<ProductBase> query = source.ToLower() switch
        {
            "amazon" => _context.AmazonProducts,
            "walmart" => _context.WalmartProducts,
            "all-stores" => _context.AllStoresProducts,
            _ => throw new ArgumentException("Unknown source")
        };

        int totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync<ProductBase>();

        return new PagedResult<ProductBase>
        {
            TotalCount = totalCount,
            Items = items
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

            case "all-stores":
                return await _context.AllStoresProducts
                    .Where(p => nameFilters.Any(n => p.Name.ToLower().Contains(n)))
                    .ToListAsync<ProductBase>();

            default:
                throw new ArgumentException("Invalid source");
        }
    }
    
    public async Task<PagedResult<ProductBase>> SearchByNamesPaged(string source, List<string> names, int page, int pageSize)
    {
        var nameFilters = names.Select(n => n.ToLower()).ToList();

        IQueryable<ProductBase> query = source.ToLower() switch
        {
            "amazon" => _context.AmazonProducts,
            "walmart" => _context.WalmartProducts,
            "all-stores" => _context.AllStoresProducts,
            _ => throw new ArgumentException("Invalid source")
        };

        query = query.Where(p => nameFilters.Any(n => p.Name.ToLower().Contains(n)));

        int totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync<ProductBase>();

        return new PagedResult<ProductBase>
        {
            TotalCount = totalCount,
            Items = items
        };
    }


    public async Task<List<ProductBase>> GetProductsFromOneStore(string name)
    {
        return await _context.AllStoresProducts
            .Where(p => p.sourceName.Equals(name.ToUpper()))
            .ToListAsync<ProductBase>();
    }
}