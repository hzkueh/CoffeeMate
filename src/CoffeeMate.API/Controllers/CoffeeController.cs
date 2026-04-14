using CoffeeMate.Application.DTOs;
using CoffeeMate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMate.API.Controllers;

public class CoffeeController(ICoffeeRepository coffeeRepository) : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var coffees = await coffeeRepository.GetAllAsync();

        var result = coffees.Select(c => new CoffeeListDto(
            c.Id, c.Name, c.Description, c.Origin, c.ImageUrl));

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var coffee = await coffeeRepository.GetByIdAsync(id);
        if (coffee is null) return NotFound();

        var result = new CoffeeDetailDto(
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

        return Ok(result);
    }
}
