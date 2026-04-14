using CoffeeMate.Application.DTOs;

namespace CoffeeMate.Application.Interfaces;

public interface ICoffeeService
{
    Task<IEnumerable<CoffeeListDto>> GetAllAsync();
    Task<CoffeeDetailDto> GetByIdAsync(Guid id);
}
