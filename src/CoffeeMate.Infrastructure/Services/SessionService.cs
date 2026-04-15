using CoffeeMate.Application.DTOs;
using CoffeeMate.Application.Exceptions;
using CoffeeMate.Application.Interfaces;
using CoffeeMate.Domain.Entities;
using CoffeeMate.Domain.Enums;
using CoffeeMate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMate.Infrastructure.Services;

public class SessionService(BaristaContext context) : ISessionService
{
    // -------------------------------------------------------------------------
    // Create
    // -------------------------------------------------------------------------

    public async Task<SessionDto> CreateAsync(CreateSessionDto dto, string userId)
    {
        var coffee = await context.Coffees
            .Include(c => c.Recipe)
                .ThenInclude(r => r!.Steps)
            .FirstOrDefaultAsync(c => c.Id == dto.CoffeeId)
            ?? throw new NotFoundException(nameof(Coffee), dto.CoffeeId);

        if (coffee.Recipe is null || !coffee.Recipe.Steps.Any())
            throw new BadRequestException(
                $"'{coffee.Name}' has no recipe steps — cannot start a session.");

        var user = await context.Users.FindAsync(userId)
            ?? throw new UnauthorizedException("Authenticated user not found.");

        var session = new Session
        {
            Id = Guid.NewGuid(),
            CoffeeId = dto.CoffeeId,
            CreatedByUserId = userId,
            Status = SessionStatus.InProgress,
            CreatedAt = DateTime.UtcNow
        };

        var sessionSteps = coffee.Recipe.Steps
            .OrderBy(s => s.Order)
            .Select(s => new SessionStep
            {
                Id = Guid.NewGuid(),
                Order = s.Order,
                Name = s.Name,
                Description = s.Description,
                Status = StepStatus.Available,
                SessionId = session.Id
            })
            .ToList();

        var collaborator = new Collaborator
        {
            Id = Guid.NewGuid(),
            DisplayName = user.DisplayName,
            UserId = userId,
            SessionId = session.Id,
            JoinedAt = DateTime.UtcNow
        };

        context.Sessions.Add(session);
        context.SessionSteps.AddRange(sessionSteps);
        context.Collaborators.Add(collaborator);
        await context.SaveChangesAsync();

        return MapToSessionDto(session, coffee.Name, sessionSteps);
    }

    // -------------------------------------------------------------------------
    // Get
    // -------------------------------------------------------------------------

    public async Task<SessionDto> GetByIdAsync(Guid sessionId)
    {
        var session = await context.Sessions
            .Include(s => s.Coffee)
            .Include(s => s.SessionSteps.OrderBy(ss => ss.Order))
            .FirstOrDefaultAsync(s => s.Id == sessionId)
            ?? throw new NotFoundException(nameof(Session), sessionId);

        return MapToSessionDto(session, session.Coffee.Name, [.. session.SessionSteps]);
    }

    // -------------------------------------------------------------------------
    // Claim Step
    // -------------------------------------------------------------------------

    public async Task<SessionStepDto> ClaimStepAsync(
        Guid sessionId, Guid stepId, string? userId, string? guestToken)
    {
        var claimerName = await ResolveClaimerNameAsync(sessionId, userId, guestToken);

        var step = await context.SessionSteps
            .FirstOrDefaultAsync(ss => ss.Id == stepId && ss.SessionId == sessionId)
            ?? throw new NotFoundException(nameof(SessionStep), stepId);

        if (step.Status != StepStatus.Available)
            throw new BadRequestException("This step has already been claimed.");

        step.Status = StepStatus.Claimed;
        step.ClaimedBy = claimerName;
        step.ClaimedAt = DateTime.UtcNow;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            // Another participant claimed this step between our read and our write
            throw new BadRequestException(
                "This step was just claimed by someone else. Please try another step.");
        }

        return MapToStepDto(step);
    }

    // -------------------------------------------------------------------------
    // Complete Step
    // -------------------------------------------------------------------------

    public async Task<SessionStepDto> CompleteStepAsync(
        Guid sessionId, Guid stepId, string? userId, string? guestToken)
    {
        var claimerName = await ResolveClaimerNameAsync(sessionId, userId, guestToken);

        var step = await context.SessionSteps
            .FirstOrDefaultAsync(ss => ss.Id == stepId && ss.SessionId == sessionId)
            ?? throw new NotFoundException(nameof(SessionStep), stepId);

        if (step.Status != StepStatus.Claimed)
            throw new BadRequestException("Only a claimed step can be completed.");

        if (step.ClaimedBy != claimerName)
            throw new BadRequestException("You can only complete a step you have claimed.");

        step.Status = StepStatus.Completed;
        step.CompletedAt = DateTime.UtcNow;

        // Auto-complete the session once every step is done
        var remainingSteps = await context.SessionSteps
            .Where(ss => ss.SessionId == sessionId && ss.Id != stepId)
            .Select(ss => ss.Status)
            .ToListAsync();

        var allDone = remainingSteps.All(s => s == StepStatus.Completed);
        if (allDone)
        {
            var session = await context.Sessions.FindAsync(sessionId);
            if (session is not null)
            {
                session.Status = SessionStatus.Completed;
                session.CompletedAt = DateTime.UtcNow;
            }
        }

        await context.SaveChangesAsync();
        return MapToStepDto(step);
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private async Task<string> ResolveClaimerNameAsync(
        Guid sessionId, string? userId, string? guestToken)
    {
        if (userId is not null)
        {
            var user = await context.Users.FindAsync(userId)
                ?? throw new UnauthorizedException("User not found.");
            return user.DisplayName;
        }

        if (guestToken is not null)
        {
            var token = await context.GuestTokens
                .FirstOrDefaultAsync(g => g.Token == guestToken && g.SessionId == sessionId)
                ?? throw new UnauthorizedException("Invalid or expired guest token.");

            if (!token.IsActive)
                throw new UnauthorizedException("Guest session has expired.");

            return token.AssignedUsername;
        }

        throw new UnauthorizedException(
            "Authentication required. Provide a JWT Bearer token or a guest token.");
    }

    private static SessionDto MapToSessionDto(
        Session session, string coffeeName, IList<SessionStep> steps) =>
        new(
            session.Id,
            session.Status.ToString(),
            session.CoffeeId,
            coffeeName,
            session.CreatedByUserId,
            session.CreatedAt,
            session.CompletedAt,
            steps.Select(MapToStepDto)
        );

    private static SessionStepDto MapToStepDto(SessionStep step) =>
        new(
            step.Id,
            step.Order,
            step.Name,
            step.Description,
            step.Status.ToString(),
            step.ClaimedBy,
            step.ClaimedAt,
            step.CompletedAt
        );
}
