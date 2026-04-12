namespace CoffeeMate.Domain.Entities;

public class GuestToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public string AssignedUsername { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public Guid SessionId { get; set; }
    public Session Session { get; set; } = null!;
}
