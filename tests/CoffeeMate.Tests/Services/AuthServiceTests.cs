using CoffeeMate.Application.DTOs;
using CoffeeMate.Application.Exceptions;
using CoffeeMate.Infrastructure.Data;
using CoffeeMate.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace CoffeeMate.Tests.Services;

public class AuthServiceTests
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        var store = Substitute.For<IUserStore<AppUser>>();
        _userManager = Substitute.For<UserManager<AppUser>>(
            store, null, null, null, null, null, null, null, null);

        _configuration = Substitute.For<IConfiguration>();
        _configuration["Jwt:Key"].Returns("coffeemate-unit-test-secret-key-min32chars!");
        _configuration["Jwt:ExpiryMinutes"].Returns("60");
        _configuration["Jwt:Issuer"].Returns("CoffeeMate");
        _configuration["Jwt:Audience"].Returns("CoffeeMate");

        _sut = new AuthService(_userManager, _configuration);
    }

    // --- RegisterAsync ---

    [Fact]
    public async Task RegisterAsync_ReturnsAuthResponseDto_WhenSuccessful()
    {
        // Arrange
        var dto = new RegisterDto("john@example.com", "Password1!", "John");

        _userManager.CreateAsync(Arg.Any<AppUser>(), dto.Password)
            .Returns(IdentityResult.Success);

        // Act
        var result = await _sut.RegisterAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Email, result.Email);
        Assert.Equal(dto.DisplayName, result.DisplayName);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task RegisterAsync_TokenIsValidJwt_WhenSuccessful()
    {
        // Arrange
        var dto = new RegisterDto("jane@example.com", "Password1!", "Jane");

        _userManager.CreateAsync(Arg.Any<AppUser>(), dto.Password)
            .Returns(IdentityResult.Success);

        // Act
        var result = await _sut.RegisterAsync(dto);

        // Assert — a valid JWT has exactly 3 dot-separated segments
        var segments = result.Token.Split('.');
        Assert.Equal(3, segments.Length);
    }

    [Fact]
    public async Task RegisterAsync_ThrowsBadRequestException_WhenCreateFails()
    {
        // Arrange
        var dto = new RegisterDto("bad@example.com", "weak", "Bad");

        var errors = new[] { new IdentityError { Description = "Password too short." } };
        _userManager.CreateAsync(Arg.Any<AppUser>(), dto.Password)
            .Returns(IdentityResult.Failed(errors));

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _sut.RegisterAsync(dto));
    }

    [Fact]
    public async Task RegisterAsync_ExceptionMessage_ContainsIdentityError_WhenCreateFails()
    {
        // Arrange
        var dto = new RegisterDto("bad@example.com", "weak", "Bad");

        var errors = new[] { new IdentityError { Description = "Password too short." } };
        _userManager.CreateAsync(Arg.Any<AppUser>(), dto.Password)
            .Returns(IdentityResult.Failed(errors));

        // Act
        var ex = await Record.ExceptionAsync(() => _sut.RegisterAsync(dto));

        // Assert
        Assert.Contains("Password too short.", ex.Message);
    }

    // --- LoginAsync ---

    [Fact]
    public async Task LoginAsync_ReturnsAuthResponseDto_WhenCredentialsAreValid()
    {
        // Arrange
        var dto = new LoginDto("john@example.com", "Password1!");
        var user = new AppUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = dto.Email,
            UserName = dto.Email,
            DisplayName = "John"
        };

        _userManager.FindByEmailAsync(dto.Email).Returns(user);
        _userManager.CheckPasswordAsync(user, dto.Password).Returns(true);

        // Act
        var result = await _sut.LoginAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Email, result.Email);
        Assert.Equal("John", result.DisplayName);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task LoginAsync_ThrowsUnauthorizedException_WhenUserNotFound()
    {
        // Arrange
        var dto = new LoginDto("ghost@example.com", "Password1!");

        _userManager.FindByEmailAsync(dto.Email).Returns((AppUser?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => _sut.LoginAsync(dto));
    }

    [Fact]
    public async Task LoginAsync_ThrowsUnauthorizedException_WhenPasswordIsWrong()
    {
        // Arrange
        var dto = new LoginDto("john@example.com", "WrongPassword!");
        var user = new AppUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = dto.Email,
            UserName = dto.Email,
            DisplayName = "John"
        };

        _userManager.FindByEmailAsync(dto.Email).Returns(user);
        _userManager.CheckPasswordAsync(user, dto.Password).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => _sut.LoginAsync(dto));
    }

    [Fact]
    public async Task LoginAsync_DoesNotRevealWhetherEmailExists_OnWrongPassword()
    {
        // Arrange — two scenarios that both throw UnauthorizedException must have the same message
        var emailNotFound = new LoginDto("ghost@example.com", "Password1!");
        var wrongPassword = new LoginDto("john@example.com", "WrongPassword!");

        var user = new AppUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = wrongPassword.Email,
            UserName = wrongPassword.Email,
            DisplayName = "John"
        };

        _userManager.FindByEmailAsync(emailNotFound.Email).Returns((AppUser?)null);
        _userManager.FindByEmailAsync(wrongPassword.Email).Returns(user);
        _userManager.CheckPasswordAsync(user, wrongPassword.Password).Returns(false);

        // Act
        var exNotFound = await Record.ExceptionAsync(() => _sut.LoginAsync(emailNotFound));
        var exWrongPw  = await Record.ExceptionAsync(() => _sut.LoginAsync(wrongPassword));

        // Assert — identical messages prevent user enumeration
        Assert.Equal(exNotFound.Message, exWrongPw.Message);
    }
}
