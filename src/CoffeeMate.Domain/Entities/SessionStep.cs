using CoffeeMate.Domain.Enums;

namespace CoffeeMate.Domain.Entities;

public class SessionStep
{
    public Guid Id { get; set; }
    public int Order { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public StepStatus Status { get; set; } = StepStatus.Available;
    public DateTime? ClaimedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Who claimed/completed this step (guest username or registered user)
    public string? ClaimedBy { get; set; }

    public Guid SessionId { get; set; }
    public Session Session { get; set; } = null!;

    // Optimistic concurrency token to prevent double-claim
    public byte[] RowVersion { get; set; } = [];
}
