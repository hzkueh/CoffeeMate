namespace CoffeeMate.Application.DTOs;

public record StepDto(
    Guid Id,
    int Order,
    string Name,
    string Description,
    int DurationSeconds
);
