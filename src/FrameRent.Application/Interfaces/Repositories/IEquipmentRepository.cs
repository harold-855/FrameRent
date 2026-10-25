using FrameRent.Domain.Entities;

namespace FrameRent.Application.Interfaces.Repositories;

public interface IEquipmentRepository
{
    Task AddAsync(Equipment equipment);
    Task<Equipment?> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<Equipment>> GetAllAsync();
    Task<IReadOnlyCollection<Equipment>> GetAllActiveAsync();
    Task<bool> ExistsAsync(Guid id);
    Task UpdateAsync(Equipment equipment);
    Task<bool> HasActiveOrDeliveredRentalsAsync(Guid equipmentId);
    Task<int> GetRentedUnitsInDateRangeAsync(Guid equipmentId, DateTime startDate, DateTime endDate);
}