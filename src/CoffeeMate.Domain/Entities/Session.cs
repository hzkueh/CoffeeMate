using CoffeeMate.Domain.Enums;

namespace CoffeeMate.Domain.Entities;

public class Session
{
    public Guid Id { get; set; }
    public SessionStatus Status { get; set; } = SessionStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    public Guid CoffeeId { get; set; }
    public Coffee Coffee { get; set; } = null!;

    // Registered user who created the session (null if concept changes later)
    public string? CreatedByUserId { get; set; }

    public ICollection<SessionStep> SessionSteps { get; set; } = [];
    public ICollection<Collaborator> Collaborators { get; set; } = [];
    public ICollection<GuestToken> GuestTokens { get; set; } = [];
}
