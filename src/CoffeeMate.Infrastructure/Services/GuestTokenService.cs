using System.Security.Cryptography;
using CoffeeMate.Application.DTOs;
using CoffeeMate.Application.Exceptions;
using CoffeeMate.Application.Interfaces;
using CoffeeMate.Domain.Entities;
using CoffeeMate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMate.Infrastructure.Services;

public class GuestTokenService(BaristaContext context) : IGuestTokenService
{
    private static readonly string[] Adjectives =
    [
        "Bold", "Smooth", "Rich", "Dark", "Fresh",
        "Warm", "Strong", "Bitter", "Sweet", "Creamy"
    ];

    private static readonly string[] Roles =
    [
        "Grinder", "Brewer", "Roaster", "Barista", "Taster",
        "Steamer", "Pourer", "Tamper", "Blender", "Sipper"
    ];

    public async Task<GuestJoinResponseDto> JoinAsync(Guid sessionId)
    {
        var session = await context.Sessions.FindAsync(sessionId)
            ?? throw new NotFoundException(nameof(Session), sessionId);

        var tokenBytes = new byte[32];
        RandomNumberGenerator.Fill(tokenBytes);
        var token = Convert.ToBase64String(tokenBytes);
        var username = GenerateUsername();

        var guestToken = new GuestToken
        {
            Token = token,
            AssignedUsername = username,
            SessionId = sessionId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        context.GuestTokens.Add(guestToken);
        await context.SaveChangesAsync();

        return new GuestJoinResponseDto(token, username);
    }

    public async Task<GuestRejoinResponseDto> RejoinAsync(Guid sessionId, GuestRejoinDto dto)
    {
        var guestToken = await context.GuestTokens
            .Include(g => g.Session)
            .FirstOrDefaultAsync(g => g.Token == dto.Token && g.SessionId == sessionId)
            ?? throw new UnauthorizedException("Invalid or expired guest token.");

        if (!guestToken.IsActive)
            throw new UnauthorizedException("Guest session has expired.");

        return new GuestRejoinResponseDto(
            guestToken.AssignedUsername,
            guestToken.Session.Status);
    }

    private static string GenerateUsername()
    {
        var rng = Random.Shared;
        var role = Roles[rng.Next(Roles.Length)];
        var number = rng.Next(10, 999);
        return $"{role}{number:D3}";
    }
}
