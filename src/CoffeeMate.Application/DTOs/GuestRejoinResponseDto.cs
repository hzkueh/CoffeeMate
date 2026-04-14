using CoffeeMate.Domain.Enums;

namespace CoffeeMate.Application.DTOs;

public record GuestRejoinResponseDto(
    string AssignedUsername,
    SessionStatus SessionStatus
);
