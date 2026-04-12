namespace CoffeeMate.Domain.Entities;

public class Recipe
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public Guid CoffeeId { get; set; }
    public Coffee Coffee { get; set; } = null!;

    public ICollection<Step> Steps { get; set; } = [];
}
