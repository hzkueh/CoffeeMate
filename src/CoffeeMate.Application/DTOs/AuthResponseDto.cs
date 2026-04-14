namespace CoffeeMate.Application.DTOs;

public record AuthResponseDto(
    string Token,
    string Email,
    string DisplayName
);
