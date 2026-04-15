namespace CoffeeMate.Application.DTOs;

/// <summary>
/// Carries the caller's identity for claim and complete step actions.
/// Registered users leave this null (identity comes from the JWT).
/// Guests provide their token issued at join time.
/// </summary>
public record SessionActionDto(string? GuestToken);
