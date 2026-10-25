using FrameRent.Domain.Entities;

namespace FrameRent.Application.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer);
    Task<Customer?> GetByIdAsync(Guid id);
    Task<Customer?> GetByEmailAsync(string email);
    Task<IReadOnlyCollection<Customer>> GetAllAsync();
    Task UpdateAsync(Customer customer);
    Task<bool> ExistsByEmailAsync(string email);
}
