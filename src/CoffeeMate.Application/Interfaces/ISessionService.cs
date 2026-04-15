using CoffeeMate.Application.DTOs;

namespace CoffeeMate.Application.Interfaces;

public interface ISessionService
{
    /// <summary>Creates a new brew session for a coffee. Caller must be a registered user.</summary>
    Task<SessionDto> CreateAsync(CreateSessionDto dto, string userId);

    /// <summary>Returns the session with all step states. Accessible to anyone in the session.</summary>
    Task<SessionDto> GetByIdAsync(Guid sessionId);

    /// <summary>
    /// Claims an available step for the caller.
    /// Pass userId (from JWT) for registered users, or guestToken for guests — not both.
    /// Throws BadRequestException if the step is already claimed (race condition).
    /// </summary>
    Task<SessionStepDto> ClaimStepAsync(Guid sessionId, Guid stepId, string? userId, string? guestToken);

    /// <summary>
    /// Marks a claimed step as completed. The caller must be the one who claimed it.
    /// Auto-completes the session when every step is done.
    /// </summary>
    Task<SessionStepDto> CompleteStepAsync(Guid sessionId, Guid stepId, string? userId, string? guestToken);
}
