using System.ComponentModel.DataAnnotations;

namespace CoffeeMate.Application.DTOs;

public record GuestRejoinDto(
    [Required] string Token
);
