namespace CoffeeMate.Application.DTOs;

public record GuestJoinResponseDto(
    string GuestToken,
    string AssignedUsername
);
