using CoffeeMate.Application.DTOs;
using CoffeeMate.Application.Exceptions;
using CoffeeMate.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMate.API.Controllers;

[AllowAnonymous]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : BaseApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            var response = await authService.RegisterAsync(dto);
            return Ok(response);
        }
        catch (BadRequestException ex)
        {
            logger.LogWarning("Registration failed for {Email}: {Message}", dto.Email, ex.Message);
            return BadRequest(new { message = "Registration failed. Please check your details and try again." });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try
        {
            var response = await authService.LoginAsync(dto);
            return Ok(response);
        }
        catch (UnauthorizedException ex)
        {
            logger.LogWarning("Login failed for {Email}: {Message}", dto.Email, ex.Message);
            return Unauthorized(new { message = "Invalid email or password." });
        }
    }
}
