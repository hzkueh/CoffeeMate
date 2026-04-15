using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using CoffeeMate.Application.DTOs;
using CoffeeMate.Application.Exceptions;
using CoffeeMate.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMate.API.Controllers;

public class SessionController(
    ISessionService sessionService,
    IGuestTokenService guestTokenService,
    ILogger<SessionController> logger) : BaseApiController
{
    // -------------------------------------------------------------------------
    // POST /api/session
    // Registered users only — creates a session and seeds SessionSteps.
    // -------------------------------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> Create(CreateSessionDto dto)
    {
        // "sub" is the standard JWT claim. In .NET 8+ JsonWebTokenHandler does not
        // auto-map "sub" → ClaimTypes.NameIdentifier, so we check both.
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                  ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { message = "Unable to identify the authenticated user." });

        try
        {
            var result = await sessionService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning("Session create failed: {Message}", ex.Message);
            return NotFound(new { message = "Coffee not found." });
        }
        catch (BadRequestException ex)
        {
            logger.LogWarning("Session create failed: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedException ex)
        {
            logger.LogWarning("Session create — auth failed: {Message}", ex.Message);
            return Unauthorized(new { message = "Authentication required." });
        }
    }

    // -------------------------------------------------------------------------
    // GET /api/session/{id}
    // Anyone can view a session (registered or guest).
    // -------------------------------------------------------------------------
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var result = await sessionService.GetByIdAsync(id);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning("Session get failed {SessionId}: {Message}", id, ex.Message);
            return NotFound(new { message = "Session not found." });
        }
    }

    // -------------------------------------------------------------------------
    // POST /api/session/{id}/join-guest
    // Guests only — issues a token and username for the session.
    // -------------------------------------------------------------------------
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
            logger.LogWarning("Guest join failed {SessionId}: {Message}", id, ex.Message);
            return NotFound(new { message = "Session not found." });
        }
    }

    // -------------------------------------------------------------------------
    // POST /api/session/{id}/rejoin
    // Guests only — validates an existing token and returns current session state.
    // -------------------------------------------------------------------------
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
            logger.LogWarning("Guest rejoin failed {SessionId}: {Message}", id, ex.Message);
            return Unauthorized(new { message = "Invalid or expired guest token." });
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning("Guest rejoin — session not found {SessionId}: {Message}", id, ex.Message);
            return NotFound(new { message = "Session not found." });
        }
    }

    // -------------------------------------------------------------------------
    // PATCH /api/session/{id}/steps/{stepId}/claim
    // Registered users: no body needed (identity from JWT).
    // Guests: body { "guestToken": "..." }.
    // Returns 409 Conflict if two users claim simultaneously.
    // -------------------------------------------------------------------------
    [AllowAnonymous]
    [HttpPatch("{id:guid}/steps/{stepId:guid}/claim")]
    public async Task<IActionResult> ClaimStep(
        Guid id, Guid stepId, [FromBody] SessionActionDto? dto)
    {
        try
        {
            var userId = User.Identity?.IsAuthenticated == true
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            var result = await sessionService.ClaimStepAsync(id, stepId, userId, dto?.GuestToken);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning("Claim step — not found {SessionId}/{StepId}: {Message}", id, stepId, ex.Message);
            return NotFound(new { message = "Session or step not found." });
        }
        catch (BadRequestException ex)
        {
            logger.LogWarning("Claim step failed {SessionId}/{StepId}: {Message}", id, stepId, ex.Message);

            // Race condition — step was claimed between read and write
            if (ex.Message.Contains("just claimed"))
                return Conflict(new { message = ex.Message });

            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedException)
        {
            return Unauthorized(new { message = "Provide a valid JWT token or guest token to claim a step." });
        }
    }

    // -------------------------------------------------------------------------
    // PATCH /api/session/{id}/steps/{stepId}/complete
    // Caller must be the one who claimed the step.
    // Auto-completes the session when all steps are done.
    // -------------------------------------------------------------------------
    [AllowAnonymous]
    [HttpPatch("{id:guid}/steps/{stepId:guid}/complete")]
    public async Task<IActionResult> CompleteStep(
        Guid id, Guid stepId, [FromBody] SessionActionDto? dto)
    {
        try
        {
            var userId = User.Identity?.IsAuthenticated == true
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            var result = await sessionService.CompleteStepAsync(id, stepId, userId, dto?.GuestToken);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning("Complete step — not found {SessionId}/{StepId}: {Message}", id, stepId, ex.Message);
            return NotFound(new { message = "Session or step not found." });
        }
        catch (BadRequestException ex)
        {
            logger.LogWarning("Complete step failed {SessionId}/{StepId}: {Message}", id, stepId, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedException)
        {
            return Unauthorized(new { message = "Provide a valid JWT token or guest token to complete a step." });
        }
    }
}
