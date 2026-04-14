using System.ComponentModel.DataAnnotations;

namespace CoffeeMate.Application.DTOs;

public record RegisterDto(
    [Required][EmailAddress] string Email,
    [Required][MinLength(6)] string Password,
    [Required] string DisplayName
);
