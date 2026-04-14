using CoffeeMate.Application.Exceptions;
using CoffeeMate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMate.API.Controllers;

public class CoffeeController(ICoffeeService coffeeService) : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var coffees = await coffeeService.GetAllAsync();
        return Ok(coffees);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var coffee = await coffeeService.GetByIdAsync(id);
            return Ok(coffee);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
