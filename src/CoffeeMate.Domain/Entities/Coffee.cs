namespace CoffeeMate.Domain.Entities;

public class Coffee
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }

    public Recipe? Recipe { get; set; }
    public ICollection<Session> Sessions { get; set; } = [];
}
