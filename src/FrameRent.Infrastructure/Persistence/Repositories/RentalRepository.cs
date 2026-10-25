using FrameRent.Application.DTOs.Reports;
using FrameRent.Application.Interfaces.Repositories;
using FrameRent.Domain.Entities;
using FrameRent.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FrameRent.Infrastructure.Persistence.Repositories;

public class RentalRepository : IRentalRepository
{
    private readonly FrameRentDbContext _context;

    public RentalRepository(FrameRentDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Rental rental)
    {
        await _context.Rentals.AddAsync(rental);
        await _context.SaveChangesAsync();
    }

    public async Task<Rental?> GetByIdAsync(Guid id)
    {
        return await _context.Rentals
            .FirstOrDefaultAsync(rental => rental.Id == id);
    }

    public async Task<IReadOnlyCollection<Rental>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.Rentals
            .AsNoTracking()
            .Where(rental => rental.CustomerId == customerId)
            .OrderByDescending(rental => rental.StartDate)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<Rental>> GetAllAsync(RentalStatus? status = null)
    {
        var query = _context.Rentals.AsNoTracking().AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(rental => rental.Status == status.Value);
        }

        return await query
            .OrderByDescending(rental => rental.StartDate)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<RentalDetail>> GetDetailsByRentalIdAsync(Guid rentalId)
    {
        return await _context.RentalDetails
            .AsNoTracking()
            .Where(detail => detail.RentalId == rentalId)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<Penalty>> GetPenaltiesByRentalIdAsync(Guid rentalId)
    {
        return await _context.Penalties
            .AsNoTracking()
            .Where(penalty => penalty.RentalId == rentalId)
            .OrderByDescending(penalty => penalty.CreatedAt)
            .ToListAsync();
    }

    public async Task<int> CountActiveOrPendingByCustomerIdAsync(Guid customerId)
    {
        return await _context.Rentals
            .CountAsync(rental =>
                rental.CustomerId == customerId &&
                (rental.Status == RentalStatus.Pendiente || rental.Status == RentalStatus.Aprobado));
    }

    public async Task UpdateAsync(Rental rental)
    {
        _context.Rentals.Update(rental);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<TopEquipmentDto>> GetTopRentedEquipmentAsync(int limit = 10)
    {
        var query =
            from detail in _context.RentalDetails.AsNoTracking()
            join equipment in _context.Equipment.AsNoTracking() on detail.EquipmentId equals equipment.Id
            group detail by new { detail.EquipmentId, equipment.Name, equipment.Category } into grouped
            orderby grouped.Sum(x => x.Quantity) descending
            select new TopEquipmentDto(
                grouped.Key.EquipmentId,
                grouped.Key.Name,
                grouped.Key.Category,
                grouped.Count(),
                grouped.Sum(x => x.Quantity));

        return await query
            .Take(limit)
            .ToListAsync();
    }
}
