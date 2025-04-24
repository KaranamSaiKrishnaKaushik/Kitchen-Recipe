using System.Net.Mail;
using AutoMapper;
using DataModels;
using DTOs;

namespace Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        /*CreateMap<Recipe, RecipeDto>();
        CreateMap<RecipeDto, Recipe>();

        CreateMap<RecipeIngredients, IngredientDto>();
        CreateMap<IngredientDto, RecipeIngredients>();*/
    }
}