using CoffeeMate.Application.DTOs;

namespace CoffeeMate.Application.Interfaces;

public interface IGuestTokenService
{
    Task<GuestJoinResponseDto> JoinAsync(Guid sessionId);
    Task<GuestRejoinResponseDto> RejoinAsync(Guid sessionId, GuestRejoinDto dto);
}
