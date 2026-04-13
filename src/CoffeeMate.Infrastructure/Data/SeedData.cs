using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using CoffeeMate.Domain.Entities;

namespace CoffeeMate.Infrastructure.Data;

public static class SeedData
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private static SeedRoot Load()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(
            "CoffeeMate.Infrastructure.Data.seed.json")
            ?? throw new FileNotFoundException("Embedded seed.json not found.");

        return JsonSerializer.Deserialize<SeedRoot>(stream, JsonOptions)
            ?? throw new InvalidOperationException("Failed to deserialize seed.json.");
    }

    public static Coffee[] GetCoffees() =>
        Load().Coffees.Select(c => new Coffee
        {
            Id = Guid.Parse(c.Id),
            Name = c.Name,
            Description = c.Description,
            Origin = c.Origin
        }).ToArray();

    public static Recipe[] GetRecipes() =>
        Load().Coffees.Select(c => new Recipe
        {
            Id = Guid.Parse(c.Recipe.Id),
            CoffeeId = Guid.Parse(c.Id),
            Title = c.Recipe.Title,
            Description = c.Recipe.Description
        }).ToArray();

    public static Step[] GetSteps() =>
        Load().Coffees.SelectMany(c =>
            c.Recipe.Steps.Select(s => new Step
            {
                Id = Guid.Parse(s.Id),
                RecipeId = Guid.Parse(c.Recipe.Id),
                Order = s.Order,
                Name = s.Name,
                Description = s.Description,
                DurationSeconds = s.DurationSeconds
            })
        ).ToArray();

    // --- Private JSON shape models ---

    private sealed class SeedRoot
    {
        public List<CoffeeSeed> Coffees { get; set; } = [];
    }

    private sealed class CoffeeSeed
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public RecipeSeed Recipe { get; set; } = null!;
    }

    private sealed class RecipeSeed
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<StepSeed> Steps { get; set; } = [];
    }

    private sealed class StepSeed
    {
        public string Id { get; set; } = string.Empty;
        public int Order { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DurationSeconds { get; set; }
    }
}
