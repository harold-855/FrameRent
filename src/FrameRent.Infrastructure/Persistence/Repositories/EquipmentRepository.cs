using FrameRent.Application.Interfaces.Repositories;
using FrameRent.Domain.Entities;
using FrameRent.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FrameRent.Infrastructure.Persistence.Repositories;

public class EquipmentRepository : IEquipmentRepository
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

    public async Task<IReadOnlyCollection<Equipment>> GetAllActiveAsync()
    {
        return await _context.Equipment
            .AsNoTracking()
            .Where(equipment => equipment.IsActive)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Equipment
            .AnyAsync(equipment => equipment.Id == id);
    }

    public async Task<bool> HasActiveOrDeliveredRentalsAsync(Guid equipmentId)
    {
        return await _context.Rentals
            .AnyAsync(rental =>
                (rental.Status == RentalStatus.Aprobado || rental.Status == RentalStatus.Entregado) &&
                rental.Details.Any(detail => detail.EquipmentId == equipmentId));
    }

    public async Task<int> GetRentedUnitsInDateRangeAsync(Guid equipmentId, DateTime from, DateTime to)
    {
        var utcFrom = DateTime.SpecifyKind(from, DateTimeKind.Utc);
        var utcTo = DateTime.SpecifyKind(to, DateTimeKind.Utc);

        return await _context.RentalDetails
            .AsNoTracking()
            .Where(detail => detail.EquipmentId == equipmentId)
            .Join(
                _context.Rentals.AsNoTracking(),
                detail => detail.RentalId,
                rental => rental.Id,
                (detail, rental) => new { detail, rental })
            .Where(x =>
                (x.rental.Status == RentalStatus.Aprobado || x.rental.Status == RentalStatus.Entregado) &&
                x.rental.StartDate <= utcTo &&
                x.rental.EndDate >= utcFrom)
            .SumAsync(x => x.detail.Quantity);
    }

    public async Task UpdateAsync(Equipment equipment)
    {
        _context.Equipment.Update(equipment);
        await _context.SaveChangesAsync();
    }
}