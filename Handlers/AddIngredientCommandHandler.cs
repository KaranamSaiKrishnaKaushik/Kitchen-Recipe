using System.Security.Authentication;
using Commands;
using Data;
using DataModels;
using Microsoft.EntityFrameworkCore;

namespace Handlers;

public class AddIngredientCommandHandler
{
    private readonly DataContext _context;

    public AddIngredientCommandHandler(DataContext context)
    {
        _context = context;
    }

    private async Task<IngredientBase> GetOrCreateBase(IngredientBase baseName)
    {
        var existingBase = await _context.IngredientBase
            .FirstOrDefaultAsync(b => b.Name == baseName.Name);

        if (existingBase != null)
            return existingBase;

        baseName.Id = Guid.NewGuid();
        _context.IngredientBase.Add(baseName);
        await _context.SaveChangesAsync();

        return baseName;
    }

    public async Task<Ingredient> HandleAddIngredient(AddIngredientCommand command)
    {
        var baseName = await GetOrCreateBase(command.BaseName);

        var existing = await _context.Ingredient
            .FirstOrDefaultAsync(x => x.BaseName.Id == baseName.Id);

        if (existing != null)
        {
            existing.Amount += command.Amount;
            if (string.IsNullOrEmpty(existing.AuthenticationUid) && !string.IsNullOrEmpty(command.AuthenticationUid))
            {
                existing.AuthenticationUid = command.AuthenticationUid;
            }
        }
        else
        {
            var newIngredient = new Ingredient
            {
                BaseName = baseName,
                Amount = command.Amount
            };
            await _context.Ingredient.AddAsync(newIngredient);
        }

        await _context.SaveChangesAsync();
        return existing ?? command.ToEntity();
    }

    public async Task<Ingredient> HandleUpdateIngredient(AddIngredientCommand command)
    {
        var baseName = await GetOrCreateBase(command.BaseName);

        var existing = await _context.Ingredient
            .FirstOrDefaultAsync(x => x.BaseName.Id == baseName.Id);

        if (existing != null)
        {
            existing.Amount = command.Amount;
            if (string.IsNullOrEmpty(existing.AuthenticationUid) && !string.IsNullOrEmpty(command.AuthenticationUid))
            {
                existing.AuthenticationUid = command.AuthenticationUid;
            }
        }
        else
        {
            var newIngredient = new Ingredient
            {
                BaseName = baseName,
                Amount = command.Amount
            };
            await _context.Ingredient.AddAsync(newIngredient);
        }

        await _context.SaveChangesAsync();
        return existing ?? command.ToEntity();
    }

    public async Task<Ingredient> HandleRemoveIngredient(AddIngredientCommand command)
    {
        var baseName = await GetOrCreateBase(command.BaseName);
        var existing = await _context.Ingredient
            .FirstOrDefaultAsync(x => x.BaseName.Id == baseName.Id);

        if (existing != null)
        {
            _context.Ingredient.Remove(existing);
        }
        await _context.SaveChangesAsync();
        return existing ?? command.ToEntity();
    }

    public async Task<List<Ingredient>> HandleAddMultipleIngredients(List<AddIngredientCommand> commands)
    {
        var results = new List<Ingredient>();

        foreach (var command in commands)
        {
            var baseName = await GetOrCreateBase(command.BaseName);

            var existing = await _context.Ingredient
                .FirstOrDefaultAsync(x => x.BaseName.Id == baseName.Id);

            if (existing != null)
            {
                existing.Amount += command.Amount;
                if (string.IsNullOrEmpty(existing.AuthenticationUid) && !string.IsNullOrEmpty(command.AuthenticationUid))
                {
                    existing.AuthenticationUid = command.AuthenticationUid;
                }
                results.Add(existing);
            }
            else
            {
                var newIngredient = new Ingredient
                {
                    BaseName = baseName,
                    Amount = command.Amount,
                    AuthenticationUid = command.AuthenticationUid
                };
                await _context.Ingredient.AddAsync(newIngredient);
                results.Add(newIngredient);
            }
        }

        await _context.SaveChangesAsync();
        return results;
    }
}

