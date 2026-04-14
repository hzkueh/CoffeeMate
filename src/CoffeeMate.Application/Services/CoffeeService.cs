using CoffeeMate.Application.DTOs;
using CoffeeMate.Application.Exceptions;
using CoffeeMate.Application.Interfaces;
using CoffeeMate.Domain.Entities;

namespace CoffeeMate.Application.Services;

public class CoffeeService(ICoffeeRepository coffeeRepository) : ICoffeeService
{
    public async Task<IEnumerable<CoffeeListDto>> GetAllAsync()
    {
        var coffees = await coffeeRepository.GetAllAsync();

        return coffees.Select(c => new CoffeeListDto(
            c.Id, c.Name, c.Description, c.Origin, c.ImageUrl));
    }

    public async Task<CoffeeDetailDto> GetByIdAsync(Guid id)
    {
        var coffee = await coffeeRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Coffee), id);

        return new CoffeeDetailDto(
            coffee.Id,
            coffee.Name,
            coffee.Description,
            coffee.Origin,
            coffee.ImageUrl,
            coffee.Recipe is null ? null : new RecipeDto(
                coffee.Recipe.Id,
                coffee.Recipe.Title,
                coffee.Recipe.Description,
                coffee.Recipe.Steps.Select(s => new StepDto(
                    s.Id, s.Order, s.Name, s.Description, s.DurationSeconds))
            )
        );
    }
}
