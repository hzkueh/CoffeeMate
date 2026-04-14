using System.ComponentModel.DataAnnotations;

namespace CoffeeMate.Application.DTOs;

public record LoginDto(
    [Required][EmailAddress] string Email,
    [Required] string Password
);
