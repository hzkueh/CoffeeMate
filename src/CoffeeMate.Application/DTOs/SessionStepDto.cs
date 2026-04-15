namespace CoffeeMate.Application.DTOs;

public record SessionStepDto(
    Guid Id,
    int Order,
    string Name,
    string Description,
    string Status,
    string? ClaimedBy,
    DateTime? ClaimedAt,
    DateTime? CompletedAt
);
