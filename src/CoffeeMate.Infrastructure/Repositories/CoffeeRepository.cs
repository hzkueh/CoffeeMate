using CoffeeMate.Application.Interfaces;
using CoffeeMate.Domain.Entities;
using CoffeeMate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMate.Infrastructure.Repositories;

public class CoffeeRepository(BaristaContext context) : ICoffeeRepository
{
    public async Task<IEnumerable<Coffee>> GetAllAsync()
    {
        return await context.Coffees
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Coffee?> GetByIdAsync(Guid id)
    {
        return await context.Coffees
            .AsNoTracking()
            .Include(c => c.Recipe)
                .ThenInclude(r => r!.Steps.OrderBy(s => s.Order))
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
