namespace CoffeeMate.Domain.Entities;

public class Step
{
    public Guid Id { get; set; }
    public int Order { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationSeconds { get; set; }

    public Guid RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;
}
