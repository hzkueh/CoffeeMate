using System.ComponentModel.DataAnnotations;

namespace CoffeeMate.Application.DTOs;

public record CreateSessionDto(
    [Required] Guid CoffeeId
);
