namespace CoffeeMate.Domain.Entities;

public class Collaborator
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    // Either a registered user ID or null for guests
    public string? UserId { get; set; }
    // Link to guest token if this is a guest collaborator
    public Guid? GuestTokenId { get; set; }
    public GuestToken? GuestToken { get; set; }

    public Guid SessionId { get; set; }
    public Session Session { get; set; } = null!;
}
