namespace CoffeeMate.Application.DTOs;

public record RecipeDto(
    Guid Id,
    string Title,
    string? Description,
    IEnumerable<StepDto> Steps
);
