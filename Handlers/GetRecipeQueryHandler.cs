using AutoMapper;
using Data;
using DTOs;
using Microsoft.EntityFrameworkCore;

namespace Handlers;

public class GetRecipeQueryHandler
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetRecipeQueryHandler(DataContext dataContext, IMapper mapper)
    {
        _context = dataContext;
        _mapper = mapper;
    }

    /*public async Task<List<RecipeDto>> Handle()
    {
        var recipe = await _context.Recipe.ToListAsync();
        return _mapper.Map<List<RecipeDto>>(recipe);
    }*/
}