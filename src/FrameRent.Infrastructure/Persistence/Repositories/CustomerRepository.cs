using FrameRent.Application.Interfaces.Repositories;
using FrameRent.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FrameRent.Infrastructure.Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly FrameRentDbContext _context;

    public CustomerRepository(FrameRentDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(customer => customer.Id == id);
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        var normalizedEmail = email.Trim();
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(customer => customer.Email == normalizedEmail);
    }

    public async Task<IReadOnlyCollection<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        var normalizedEmail = email.Trim();
        return await _context.Customers
            .AnyAsync(customer => customer.Email == normalizedEmail);
    }
}
