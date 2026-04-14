using CoffeeMate.Application.DTOs;
using CoffeeMate.Application.Exceptions;
using CoffeeMate.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMate.API.Controllers;

public class SessionController(IGuestTokenService guestTokenService, ILogger<SessionController> logger) : BaseApiController
{
    // POST /api/session — registered users only (Phase 7)
    [HttpPost]
    public IActionResult Create()
    {
        // Full implementation in Phase 7 (Simulation Mode)
        return StatusCode(501, "Session creation coming in Phase 7.");
    }

    // POST /api/session/{id}/join-guest — open, no JWT required
    [AllowAnonymous]
    [HttpPost("{id:guid}/join-guest")]
    public async Task<IActionResult> JoinAsGuest(Guid id)
    {
        try
        {
            var result = await guestTokenService.JoinAsync(id);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning("Guest join failed for session {SessionId}: {Message}", id, ex.Message);
            return NotFound(new { message = "Session not found." });
        }
    }

    // POST /api/session/{id}/rejoin — open, validated via GuestToken
    [AllowAnonymous]
    [HttpPost("{id:guid}/rejoin")]
    public async Task<IActionResult> Rejoin(Guid id, GuestRejoinDto dto)
    {
        try
        {
            var result = await guestTokenService.RejoinAsync(id, dto);
            return Ok(result);
        }
        catch (UnauthorizedException ex)
        {
            logger.LogWarning("Guest rejoin failed for session {SessionId}: {Message}", id, ex.Message);
            return Unauthorized(new { message = "Invalid or expired guest token." });
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning("Guest rejoin — session not found {SessionId}: {Message}", id, ex.Message);
            return NotFound(new { message = "Session not found." });
        }
    }
}
