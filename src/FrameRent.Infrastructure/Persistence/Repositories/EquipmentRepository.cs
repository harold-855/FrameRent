
using FrameRent.Application.Interfaces.Repositories;
using FrameRent.Domain.Entities;
using FrameRent.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace FrameRent.Infrastructure.Persistence.Repositories;

public class EquipmentRepository: IEquipmentRepository
{
    private readonly FrameRentDbContext _context;

    public EquipmentRepository(FrameRentDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Equipment equipment)
    {
        await _context.Equipment.AddAsync(equipment);
        await _context.SaveChangesAsync();
    }

    public async Task<Equipment?> GetByIdAsync(Guid id)
    {
        return await _context.Equipment
            .FirstOrDefaultAsync(equipment => equipment.Id == id);
    }

    public async Task<IReadOnlyCollection<Equipment>> GetAllAsync()
    {
        return await _context.Equipment
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Equipment
            .AnyAsync(equipment => equipment.Id == id);
    }

    public async Task UpdateAsync(Equipment equipment)
    {
        _context.Equipment.Update(equipment);
        await _context.SaveChangesAsync();
    }
}