using System;
using System.Text.Json;
using CoffeeMate.Domain.Entities;

namespace CoffeeMate.Infrastructure.Data;

public class BaristaContextSeed
{
    public static async Task SeedAsync(BaristaContext context)
    {
        if (!context.Coffees.Any())
        {
            var coffeeData = await File.ReadAllTextAsync("../CoffeeMate.Infrastructure/Data/seed.json");

            var coffees = JsonSerializer.Deserialize<List<Coffee>>(coffeeData);

            if(coffees == null) return;
            context.Coffees.AddRange(coffees);
            await context.SaveChangesAsync();
        }
    }
}
