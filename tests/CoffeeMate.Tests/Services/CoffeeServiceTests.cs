using CoffeeMate.Application.Exceptions;
using CoffeeMate.Application.Interfaces;
using CoffeeMate.Application.Services;
using CoffeeMate.Domain.Entities;
using NSubstitute;

namespace CoffeeMate.Tests.Services;

public class CoffeeServiceTests
{
    private readonly ICoffeeRepository _repository = Substitute.For<ICoffeeRepository>();
    private readonly CoffeeService _sut;

    public CoffeeServiceTests()
    {
        _sut = new CoffeeService(_repository);
    }

    // --- GetAllAsync ---

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        // Arrange
        var coffees = new List<Coffee>
        {
            new() { Id = Guid.NewGuid(), Name = "Espresso", Description = "Bold", Origin = "Italy" },
            new() { Id = Guid.NewGuid(), Name = "Latte", Description = "Smooth", Origin = "Italy" }
        };
        _repository.GetAllAsync().Returns(coffees);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, c => c.Name == "Espresso");
        Assert.Contains(result, c => c.Name == "Latte");
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmpty_WhenNoCoffees()
    {
        // Arrange
        _repository.GetAllAsync().Returns([]);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.Empty(result);
    }

    // --- GetByIdAsync ---

    [Fact]
    public async Task GetByIdAsync_ReturnsCoffeeDetailDto_WhenFound()
    {
        // Arrange
        var coffeeId = Guid.NewGuid();
        var recipeId = Guid.NewGuid();
        var coffee = new Coffee
        {
            Id = coffeeId,
            Name = "Americano",
            Description = "Diluted espresso",
            Origin = "United States",
            Recipe = new Recipe
            {
                Id = recipeId,
                Title = "Classic Americano",
                CoffeeId = coffeeId,
                Steps =
                [
                    new Step { Id = Guid.NewGuid(), Order = 1, Name = "Grind Beans", Description = "Grind 18g", DurationSeconds = 30, RecipeId = recipeId },
                    new Step { Id = Guid.NewGuid(), Order = 2, Name = "Pull Shot",   Description = "Extract",   DurationSeconds = 30, RecipeId = recipeId }
                ]
            }
        };
        _repository.GetByIdAsync(coffeeId).Returns(coffee);

        // Act
        var result = await _sut.GetByIdAsync(coffeeId);

        // Assert
        Assert.Equal(coffeeId, result.Id);
        Assert.Equal("Americano", result.Name);
        Assert.NotNull(result.Recipe);
        Assert.Equal("Classic Americano", result.Recipe.Title);
        Assert.Equal(2, result.Recipe.Steps.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNullRecipe_WhenCoffeeHasNoRecipe()
    {
        // Arrange
        var coffeeId = Guid.NewGuid();
        var coffee = new Coffee
        {
            Id = coffeeId,
            Name = "Mystery Coffee",
            Description = "No recipe yet",
            Origin = "Unknown",
            Recipe = null
        };
        _repository.GetByIdAsync(coffeeId).Returns(coffee);

        // Act
        var result = await _sut.GetByIdAsync(coffeeId);

        // Assert
        Assert.Equal(coffeeId, result.Id);
        Assert.Null(result.Recipe);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsNotFoundException_WhenNotFound()
    {
        // Arrange
        var unknownId = Guid.NewGuid();
        _repository.GetByIdAsync(unknownId).Returns((Coffee?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.GetByIdAsync(unknownId));
    }

    [Fact]
    public async Task GetByIdAsync_ExceptionMessage_ContainsId()
    {
        // Arrange
        var unknownId = Guid.NewGuid();
        _repository.GetByIdAsync(unknownId).Returns((Coffee?)null);

        // Act
        var ex = await Record.ExceptionAsync(() => _sut.GetByIdAsync(unknownId));

        // Assert
        Assert.Contains(unknownId.ToString(), ex.Message);
    }
}
