using AutoMapper;
using Data;
using DTOs;
using Microsoft.EntityFrameworkCore;

namespace Handlers;

public class GetIngredientQueryHandler
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetIngredientQueryHandler(DataContext dataContext, IMapper mapper)
    {
        _context = dataContext;
        _mapper = mapper;
    }

    public async Task<List<IngredientDto>> Handle()
    {
        var ingredients = await _context.Ingredient.ToListAsync();
        return _mapper.Map<List<IngredientDto>>(ingredients);
    }
}