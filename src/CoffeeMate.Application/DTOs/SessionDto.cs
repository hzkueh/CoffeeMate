namespace CoffeeMate.Application.DTOs;

public record SessionDto(
    Guid Id,
    string Status,
    Guid CoffeeId,
    string CoffeeName,
    string? CreatedByUserId,
    DateTime CreatedAt,
    DateTime? CompletedAt,
    IEnumerable<SessionStepDto> Steps
);
