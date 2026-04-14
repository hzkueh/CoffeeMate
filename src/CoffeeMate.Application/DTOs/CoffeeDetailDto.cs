namespace CoffeeMate.Application.DTOs;

public record CoffeeDetailDto(
    Guid Id,
    string Name,
    string Description,
    string Origin,
    string? ImageUrl,
    RecipeDto? Recipe
);
