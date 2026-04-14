using CoffeeMate.Domain.Entities;

namespace CoffeeMate.Application.Interfaces;

public interface ICoffeeRepository
{
    Task<IEnumerable<Coffee>> GetAllAsync();
    Task<Coffee?> GetByIdAsync(Guid id);
}
