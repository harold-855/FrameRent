using FrameRent.Application.DTOs.Reports;
using FrameRent.Domain.Entities;
using FrameRent.Domain.Enums;

namespace FrameRent.Application.Interfaces.Repositories;

public interface IRentalRepository
{
    Task AddAsync(Rental rental);
    Task<Rental?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<Rental>> GetByCustomerIdAsync(Guid customerId);
    Task<IReadOnlyCollection<Rental>> GetAllAsync(RentalStatus? status = null);
    Task<int> CountActiveOrPendingByCustomerIdAsync(Guid customerId);
    Task UpdateAsync(Rental rental);
    Task<IReadOnlyCollection<TopEquipmentDto>> GetTopRentedEquipmentAsync(int limit = 10);
}
