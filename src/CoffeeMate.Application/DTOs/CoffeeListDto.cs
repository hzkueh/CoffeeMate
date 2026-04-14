namespace CoffeeMate.Application.DTOs;

public record CoffeeListDto(
    Guid Id,
    string Name,
    string Description,
    string Origin,
    string? ImageUrl
);
